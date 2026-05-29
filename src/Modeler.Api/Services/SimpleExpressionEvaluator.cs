using System.Collections.Generic;
using System.Globalization;

namespace Modeler.Api.Services;

/// <summary>
/// Minimal expression evaluator for routing rules.
/// Supports:
/// - literals: numbers, true/false, strings in single or double quotes
/// - identifiers: FactKey (looked up from provided dictionary)
/// - operators: ==, !=, >, <, >=, <=, &&, ||, !
/// - parentheses
/// Also supports "and"/"or" keywords.
/// </summary>
public sealed class SimpleExpressionEvaluator
{
    // NOTE: use IDictionary to avoid interface conversion issues across TFMs / nullability.
    private readonly IDictionary<string, string?> _facts;

    public SimpleExpressionEvaluator(IDictionary<string, string?> facts)
    {
        _facts = facts;
    }

    public bool EvaluateBoolean(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return true;

        var tokens = Tokenize(expression);
        var rpn = ToRpn(tokens);
        var result = EvalRpn(rpn);
        return ToBool(result);
    }

    // ---------------- Tokenization ----------------
    private enum TokKind { Ident, Number, String, Bool, Op, LParen, RParen }

    private sealed record Tok(TokKind Kind, string Text);

    private static List<Tok> Tokenize(string s)
    {
        var list = new List<Tok>();
        int i = 0;
        while (i < s.Length)
        {
            char c = s[i];
            if (char.IsWhiteSpace(c)) { i++; continue; }

            if (c == '(') { list.Add(new Tok(TokKind.LParen, "(")); i++; continue; }
            if (c == ')') { list.Add(new Tok(TokKind.RParen, ")")); i++; continue; }

            // operators (2-char first)
            if (i + 1 < s.Length)
            {
                var two = s.Substring(i, 2);
                if (two is "==" or "!=" or ">=" or "<=" or "&&" or "||")
                {
                    list.Add(new Tok(TokKind.Op, two));
                    i += 2;
                    continue;
                }
            }
            if (c is '>' or '<' or '!')
            {
                list.Add(new Tok(TokKind.Op, c.ToString()));
                i++;
                continue;
            }

            // string literal
            if (c is '\'' or '"')
            {
                var quote = c;
                i++;
                var start = i;
                var sb = new System.Text.StringBuilder();
                while (i < s.Length)
                {
                    if (s[i] == '\\' && i + 1 < s.Length)
                    {
                        // basic escapes
                        i++;
                        sb.Append(s[i]);
                        i++;
                        continue;
                    }
                    if (s[i] == quote)
                        break;
                    sb.Append(s[i]);
                    i++;
                }
                if (i >= s.Length || s[i] != quote)
                    throw new FormatException("Unterminated string literal");
                i++; // closing quote
                list.Add(new Tok(TokKind.String, sb.ToString()));
                continue;
            }

            // number
            if (char.IsDigit(c) || (c == '.' && i + 1 < s.Length && char.IsDigit(s[i + 1])))
            {
                var start = i;
                i++;
                while (i < s.Length && (char.IsDigit(s[i]) || s[i] == '.')) i++;
                list.Add(new Tok(TokKind.Number, s[start..i]));
                continue;
            }

            // identifier / keyword
            if (char.IsLetter(c) || c == '_' )
            {
                var start = i;
                i++;
                while (i < s.Length && (char.IsLetterOrDigit(s[i]) || s[i] == '_' || s[i] == '.')) i++;
                var text = s[start..i];
                var lower = text.ToLowerInvariant();
                if (lower is "true" or "false")
                {
                    list.Add(new Tok(TokKind.Bool, lower));
                }
                else if (lower == "and")
                {
                    list.Add(new Tok(TokKind.Op, "&&"));
                }
                else if (lower == "or")
                {
                    list.Add(new Tok(TokKind.Op, "||"));
                }
                else
                {
                    list.Add(new Tok(TokKind.Ident, text));
                }
                continue;
            }

            throw new FormatException($"Unexpected character '{c}' in expression");
        }
        return list;
    }

    // ---------------- Shunting-yard to RPN ----------------
    private static int Prec(string op) => op switch
    {
        "!" => 5,
        ">" or "<" or ">=" or "<=" => 4,
        "==" or "!=" => 3,
        "&&" => 2,
        "||" => 1,
        _ => 0
    };

    private static bool IsRightAssoc(string op) => op == "!";

    private static List<Tok> ToRpn(List<Tok> tokens)
    {
        var output = new List<Tok>();
        var ops = new Stack<Tok>();

        foreach (var t in tokens)
        {
            switch (t.Kind)
            {
                case TokKind.Ident:
                case TokKind.Number:
                case TokKind.String:
                case TokKind.Bool:
                    output.Add(t);
                    break;
                case TokKind.Op:
                    while (ops.Count > 0 && ops.Peek().Kind == TokKind.Op)
                    {
                        var top = ops.Peek().Text;
                        var pTop = Prec(top);
                        var pCur = Prec(t.Text);
                        if (pTop > pCur || (pTop == pCur && !IsRightAssoc(t.Text)))
                            output.Add(ops.Pop());
                        else
                            break;
                    }
                    ops.Push(t);
                    break;
                case TokKind.LParen:
                    ops.Push(t);
                    break;
                case TokKind.RParen:
                    while (ops.Count > 0 && ops.Peek().Kind != TokKind.LParen)
                        output.Add(ops.Pop());
                    if (ops.Count == 0 || ops.Peek().Kind != TokKind.LParen)
                        throw new FormatException("Mismatched parentheses");
                    ops.Pop();
                    break;
            }
        }

        while (ops.Count > 0)
        {
            var t = ops.Pop();
            if (t.Kind is TokKind.LParen or TokKind.RParen)
                throw new FormatException("Mismatched parentheses");
            output.Add(t);
        }

        return output;
    }

    // ---------------- RPN evaluation ----------------
    private object? EvalRpn(List<Tok> rpn)
    {
        var st = new Stack<object?>();
        foreach (var t in rpn)
        {
            if (t.Kind == TokKind.Op)
            {
                if (t.Text == "!")
                {
                    var a = st.Pop();
                    st.Push(!ToBool(a));
                    continue;
                }

                var right = st.Pop();
                var left = st.Pop();
                st.Push(ApplyBinary(t.Text, left, right));
                continue;
            }

            st.Push(t.Kind switch
            {
                TokKind.Bool => string.Equals(t.Text, "true", StringComparison.OrdinalIgnoreCase),
                TokKind.Number => decimal.Parse(t.Text, CultureInfo.InvariantCulture),
                TokKind.String => t.Text,
                TokKind.Ident => ResolveIdent(t.Text),
                _ => null
            });
        }
        return st.Count == 0 ? null : st.Pop();
    }

    private object? ResolveIdent(string ident)
    {
        if (!_facts.TryGetValue(ident, out var v) || v is null)
            return null;

        // try bool
        if (bool.TryParse(v, out var b)) return b;
        // try decimal
        if (decimal.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out var d)) return d;
        // keep as string
        return v;
    }

    private static object ApplyBinary(string op, object? left, object? right)
    {
        return op switch
        {
            "&&" => ToBool(left) && ToBool(right),
            "||" => ToBool(left) || ToBool(right),
            "==" => Compare(left, right) == 0,
            "!=" => Compare(left, right) != 0,
            ">" => Compare(left, right) > 0,
            "<" => Compare(left, right) < 0,
            ">=" => Compare(left, right) >= 0,
            "<=" => Compare(left, right) <= 0,
            _ => throw new NotSupportedException($"Unsupported operator: {op}")
        };
    }

    private static int Compare(object? a, object? b)
    {
        if (a is null && b is null) return 0;
        if (a is null) return -1;
        if (b is null) return 1;

        // numeric
        if (a is decimal da && b is decimal db)
            return da.CompareTo(db);

        // bool
        if (a is bool ba && b is bool bb)
            return ba.CompareTo(bb);

        // mixed numeric
        if (a is decimal da2 && b is not decimal)
        {
            if (TryToDecimal(b, out var db2)) return da2.CompareTo(db2);
        }
        if (b is decimal db3 && a is not decimal)
        {
            if (TryToDecimal(a, out var da3)) return da3.CompareTo(db3);
        }

        // string compare (case-insensitive)
        var sa = a.ToString() ?? "";
        var sb = b.ToString() ?? "";
        return string.Compare(sa, sb, StringComparison.OrdinalIgnoreCase);
    }

    private static bool TryToDecimal(object? o, out decimal d)
    {
        d = 0;
        if (o is null) return false;
        if (o is decimal dd) { d = dd; return true; }
        return decimal.TryParse(o.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d);
    }

    private static bool ToBool(object? v)
    {
        if (v is null) return false;
        if (v is bool b) return b;
        if (v is decimal d) return d != 0m;
        if (bool.TryParse(v.ToString(), out var bb)) return bb;
        return !string.IsNullOrWhiteSpace(v.ToString());
    }
}
