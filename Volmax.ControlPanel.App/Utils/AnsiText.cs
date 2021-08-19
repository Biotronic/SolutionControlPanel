using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Volmax.ControlPanel.App.Utils
{
    public class AnsiText
    {
        public List<Color> Colors { get; } = new List<Color>
        {
            Color.Black,
            Color.DarkRed,
            Color.DarkGreen,
            Color.Olive,
            Color.DarkBlue,
            Color.DarkMagenta,
            Color.DarkCyan,
            Color.Silver,
            Color.Gray,
            Color.Red,
            Color.Green,
            Color.Yellow,
            Color.Blue,
            Color.Magenta,
            Color.Cyan,
            Color.White
        };

        public List<Font> Fonts { get; } = new List<Font>
        {
            new Font(FontFamily.GenericMonospace, 12),
            new Font(FontFamily.GenericSansSerif, 12),
            new Font(FontFamily.GenericMonospace, 12),
            new Font(FontFamily.GenericSerif, 12),
            new Font(FontFamily.GenericSerif, 12),
            new Font(FontFamily.GenericSerif, 12),
            new Font(FontFamily.GenericSerif, 12),
            new Font(FontFamily.GenericSerif, 12),
            new Font(FontFamily.GenericSerif, 12),
            new Font(FontFamily.GenericSerif, 12),
            new Font(FontFamily.GenericMonospace, 12)
        };

        private string ColorTable => $@"{{\colortbl ;{string.Join("", Colors.Skip(1).Select(c => $@"\red{c.R}\green{c.G}\blue{c.B};"))}}}";
        private string FontTable => $@"{{\fonttbl{string.Join("", Fonts.Select((f, i) => $@"{{\f{i} {f.Name};}}"))}}}";
        private string Header => $@"{{\rtf1{FontTable}{ColorTable}";
        private string Footer => "}";

        private const string Reset = @"\plain ";

        private Trie<char, string> Replacements { get; }
        private StringBuilder Builder { get; }

        public AnsiText()
        {
            Builder = new StringBuilder();
            Replacements = new Trie<char, string>
            {
                {"ï½£", $"\\u{(int) '｣'}?"},
                {"ï½¢", $"\\u{(int) '｢'}?"},
                {"Ã—",$"\\u{(int)'×'}?" }
            };
        }

        public override string ToString()
        {
            // StringBuilder is not thread safe
            lock (Builder)
            {
                return $"{Header}{Builder}{Footer}";
            }
        }

        private void BuilderAppend(string s)
        {
            // StringBuilder is not thread safe
            lock (Builder)
            {
                Builder.Append(s);
            }
        }

        private void BuilderAppend(char c)
        {
            // StringBuilder is not thread safe
            lock (Builder)
            {
                Builder.Append(c);
            }
        }

        public void Red()
        {
            BuilderAppend(@"\cf1 ");
        }

        public void Black()
        {
            BuilderAppend(@"\cf0 ");
        }

        public void Newline()
        {
            BuilderAppend(@"\line ");
        }

        public void Append(string s)
        {
            for (var i = 0; i < s.Length; ++i)
            {
                if (s[i] == '\u001b')
                {
                    var next = Math.Min(i + 1, s.Length);
                    ++i;
                    switch (s[next])
                    {
                        case '[':
                            ++i;
                            i = ControlSequence(s, i);
                            break;
                        case 'N':
                            Trace.WriteLine("Unsupported escape sequence SS2");
                            break;
                        case 'O':
                            Trace.WriteLine("Unsupported escape sequence SS3");
                            break;
                        case 'P':
                            Trace.WriteLine("Unsupported escape sequence DCS");
                            break;
                        case ']':
                            Trace.WriteLine("Unsupported escape sequence OSC");
                            ++i;
                            i = ReadUntilSt(s, i);
                            break;
                        case 'X':
                            Trace.WriteLine("Unsupported escape sequence SOS");
                            ++i;
                            i = ReadUntilSt(s, i);
                            break;
                        case '^':
                            Trace.WriteLine("Unsupported escape sequence PM");
                            ++i;
                            i = ReadUntilSt(s, i);
                            break;
                        case '_':
                            Trace.WriteLine("Unsupported escape sequence APC");
                            ++i;
                            i = ReadUntilSt(s, i);
                            break;
                        case 'c':
                            BuilderAppend(Reset);
                            break;
                        default:
                            Trace.WriteLine($"Unsupported escape sequence {s[next]}");
                            break;
                    }
                }
                else
                {
                    var prefix = string.Concat(Replacements.LongestPrefix(s.Skip(i)));
                    if (Replacements.ContainsKey(prefix))
                    {
                        BuilderAppend(Replacements[prefix]);
                        i += prefix.Length - 1;
                        continue;
                    }
                    var c = s[i];
                    if (c >= 0x20 && c < 0x80)
                    {
                        if (@"\{}".Contains(c))
                        {
                            BuilderAppend(@"\");
                        }
                        BuilderAppend(c);
                    }
                    else if (c < 0x20 || c >= 0x80 && c <= 0xFF)
                    {
                        switch (c)
                        {
                            case '\t':
                                BuilderAppend(@"\tab ");
                                break;
                            case '\r':
                                break;
                            case '\n':
                                BuilderAppend(@"\line ");
                                break;
                            default:
                                BuilderAppend($"\\'{(byte)c:X}");
                                break;
                        }
                    }
                    else
                    {
                        BuilderAppend($"\\u{(short)c}?");
                    }
                }
            }
        }

        private static int ReadUntilSt(string s, int i)
        {
            return s.IndexOf("\u001b\\", i, StringComparison.Ordinal);
        }

        private int ControlSequence(string s, int i)
        {
            var args = "";
            while (i < s.Length && (char.IsNumber(s[i]) || s[i] == ';'))
            {
                args += s[i];
                ++i;
            }

            if (i >= s.Length)
            {
                if (args.Any())
                {
                    Trace.WriteLine($"Malformed escape sequence CSI {args}");
                }
                return i;
            }

            switch (s[i])
            {
                case 'm':
                    BuilderAppend(SelectGraphicRendition(args.Split(';').Select(int.Parse).ToArray()));
                    break;
                case 'A':
                    Trace.WriteLine("Unsupported escape sequence CSI CUU");
                    break;
                case 'B':
                    Trace.WriteLine("Unsupported escape sequence CSI CUD");
                    break;
                case 'C':
                    Trace.WriteLine("Unsupported escape sequence CSI CUF");
                    break;
                case 'D':
                    Trace.WriteLine("Unsupported escape sequence CSI CUB");
                    break;
                case 'E':
                    Trace.WriteLine("Unsupported escape sequence CSI CNL");
                    break;
                case 'F':
                    Trace.WriteLine("Unsupported escape sequence CSI CPL");
                    break;
                case 'G':
                    Trace.WriteLine("Unsupported escape sequence CSI CHA");
                    break;
                case 'H':
                    Trace.WriteLine("Unsupported escape sequence CSI CUP");
                    break;
                case 'J':
                    Trace.WriteLine("Unsupported escape sequence CSI ED");
                    break;
                case 'K':
                    Trace.WriteLine("Unsupported escape sequence CSI EL");
                    break;
                case 'S':
                    Trace.WriteLine("Unsupported escape sequence CSI SU");
                    break;
                case 'T':
                    Trace.WriteLine("Unsupported escape sequence CSI SD");
                    break;
                case 'f':
                    Trace.WriteLine("Unsupported escape sequence CSI HVP");
                    break;
                case 'i':
                    Trace.WriteLine("Unsupported escape sequence CSI AUX");
                    break;
                case 'n':
                    Trace.WriteLine("Unsupported escape sequence CSI DSR");
                    break;
                case 's':
                    Trace.WriteLine("Unsupported escape sequence CSI SCP");
                    break;
                case 'u':
                    Trace.WriteLine("Unsupported escape sequence CSI RCP");
                    break;
                default:
                    Trace.WriteLine($"Unsupported escape sequence CSI '{s[i]}'");
                    break;
            }

            return i;
        }

        private static string SelectGraphicRendition(IReadOnlyList<int> args)
        {
            if (args.Count == 0) return "";
            switch (args[0])
            {
                case 0:
                    // Default rendition
                    return Reset;
                case 1:
                    // bold or increased intensity
                    return @"\b ";
                case 2:
                    // faint, decreased intensity
                    return @"\b0 ";
                case 3:
                    // italicized
                    return @"\i ";
                case 4:
                    // singly underlined
                    return @"\ul ";
                case 5:
                    // slowly blinking (less then 150 per minute)
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Slow Blink");
                    return "";
                case 6:
                    // rapidly blinking (150 per minute or more)
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Fast Blink");
                    return "";
                case 7:
                    // negative image
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Reverse Video");
                    return "";
                case 8:
                    // concealed characters
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Conceal");
                    return "";
                case 9:
                    // crossed-out
                    return @"\strike ";
                case 21:
                    // set normal intensity
                    return @"\b0 ";
                case 22:
                    // normal color or normal intensity
                    return @"\b0 ";
                case 23:
                    // not italicized, not fraktur
                    return @"\ulnone ";
                case 24:
                    // not underlined
                    return @"\ulnone ";
                case 25:
                    // steady (not blinking)
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Blink Off");
                    return "";
                case 26:
                    // proportional spacing
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Proportional Spacing");
                    return "";
                case 27:
                    // positive image
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Inverse Off");
                    return "";
                case 28:
                    // revealed characters
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Reveal");
                    return "";
                case 29:
                    // not crossed out
                    return @"\strike0 ";
                case 39:
                    // default display color
                    return @" \cf0 ";
                case 50:
                    // Reset to the original color pair
                    return @"\cf0 \highlight0 ";
                case 51:
                    // Framed
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Framed");
                    return "";
                case 52:
                    // encircled
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Encircled");
                    return "";
                case 53:
                    // overlined
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Overlined");
                    return "";
                case 54:
                    // not framed, not encircled
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Not Framed, Not Encircled");
                    return "";
                case 55:
                    // not overlined
                    Trace.WriteLine("Unsupported escape sequence CSI SGR Overlined");
                    return "";
                case 56:
                case 57:
                case 58:
                case 59:
                    //(reserved for future standardization)
                    Trace.WriteLine($"Unsupported escape sequence CSI SGR {args[0]} - Reserved for Future Standardization");
                    return "";
                case 60:
                    // ideogram underline or right side line
                    Trace.WriteLine("Unsupported escape sequence CSI SGR 60 - Ideogram Underline");
                    return "";
                case 61:
                    // ideogram double underline or double line on the right side
                    Trace.WriteLine("Unsupported escape sequence CSI SGR 61 - Ideogram Double Underline");
                    return "";
                case 62:
                    // ideogram overline or left side line
                    Trace.WriteLine("Unsupported escape sequence CSI SGR 62 - Ideogram Overline");
                    return "";
                case 63:
                    // ideogram double overline or double line on the left side
                    Trace.WriteLine("Unsupported escape sequence CSI SGR 63 - Ideogram Double Overline");
                    return "";
                case 64:
                    // ideogram stress marking
                    Trace.WriteLine("Unsupported escape sequence CSI SGR 64 - Stress Marking");
                    return "";
                case 65:
                    // cancels the effect of the rendition aspects established by parameter values 60 to 64
                    Trace.WriteLine("Unsupported escape sequence CSI SGR 65 - Ideogram Reset");
                    return "";

                case 38:
                    if (args.Count == 1)
                    {
                        // Reserved for Future Use
                        Trace.WriteLine("Unsupported escape sequence CSI SGR 38 - Reserved for Future Use");
                        return "";
                    }

                    return "";
                case 48:
                    if (args.Count == 1)
                    {
                        // Reserved for Future Use
                        Trace.WriteLine("Unsupported escape sequence CSI SGR 38 - Reserved for Future Use");
                        return "";
                    }
                    return "";
                default:
                    if (args[0] >= 10 && args[0] <= 20)
                    {
                        // Fonts
                        return $@"\f{args[0] - 10} ";
                    }
                    if (args[0] >= 30 && args[0] < 38)
                    {
                        // Foreground color
                        return $@"\cf{args[0] - 30} ";
                    }
                    if (args[0] >= 90 && args[0] < 98)
                    {
                        // Foreground color (aixterm/SCOANSI)
                        return $@"\cf{args[0] - 90} ";
                    }
                    if (args[0] >= 40 && args[0] < 48)
                    {
                        // Background color
                        return $@"\cf{args[0] - 40} ";
                    }
                    if (args[0] >= 100 && args[0] < 108)
                    {
                        // Background color (aixterm/SCOANSI)
                        return $@"\highlight{args[0] - 100} ";
                    }

                    Trace.WriteLine($"Unsupported escape sequence CSI SGR {string.Join(";", args.Select(a => a.ToString()))}m");
                    return "";
            }
        }

        public void Clear()
        {
            lock (Builder)
            {
                Builder.Clear();
            }
        }

        public void CopyTo(AnsiText richText)
        {
            richText.Colors.Clear();
            richText.Colors.AddRange(Colors);
            richText.Fonts.Clear();
            richText.Fonts.AddRange(Fonts);
            lock (richText.Builder)
            {
                richText.Builder.Clear();
                richText.Builder.Append(Builder.ToString());
            }
        }
    }
}