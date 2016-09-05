using System;
using System.Text;

namespace iTextSharp.text.pdf
{
    /// <summary>
    /// Implements the code 128 and UCC/EAN-128. Other symbologies are allowed in raw mode.
    /// The code types allowed are:
    ///
    ///   CODE128  - plain barcode 128.
    ///   CODE128_UCC  - support for UCC/EAN-128 with a full list of AI.
    ///   CODE128_RAW  - raw mode. The code attribute has the actual codes from 0
    /// to 105 followed by '&#92;uffff' and the human readable text.
    ///
    /// The default parameters are:
    ///
    /// x = 0.8f;
    /// font = BaseFont.CreateFont("Helvetica", "winansi", false);
    /// size = 8;
    /// baseline = size;
    /// barHeight = size * 3;
    /// textint= Element.ALIGN_CENTER;
    /// codeType = CODE128;
    ///
    /// @author Paulo Soares (psoares@consiste.pt)
    /// </summary>
    public class Barcode128 : Barcode
    {

        public const char CODE_A = '\u00c8';

        /// <summary>
        /// The charset code change.
        /// </summary>
        public const char CODE_AB_TO_C = (char)99;

        /// <summary>
        /// The charset code change.
        /// </summary>
        public const char CODE_AC_TO_B = (char)100;

        /// <summary>
        /// The charset code change.
        /// </summary>
        public const char CODE_BC_TO_A = (char)101;

        public const char CODE_C = '\u00c7';

        public const char DEL = '\u00c3';

        public const char FNC1 = '\u00ca';

        /// <summary>
        /// The code for UCC/EAN-128.
        /// </summary>
        public const char FNC1_INDEX = (char)102;

        public const char FNC2 = '\u00c5';

        public const char FNC3 = '\u00c4';

        public const char FNC4 = '\u00c8';

        public const char SHIFT = '\u00c6';

        /// <summary>
        /// The start code.
        /// </summary>
        public const char START_A = (char)103;

        /// <summary>
        /// The start code.
        /// </summary>
        public const char START_B = (char)104;

        /// <summary>
        /// The start code.
        /// </summary>
        public const char START_C = (char)105;

        public const char STARTA = '\u00cb';

        public const char STARTB = '\u00cc';

        public const char STARTC = '\u00cd';

        private static readonly IntHashtable _ais = new IntHashtable();

        /// <summary>
        /// The bars to generate the code.
        /// </summary>
        private static readonly byte[][] _bars =
        {
            new byte[] {2, 1, 2, 2, 2, 2},
            new byte[] {2, 2, 2, 1, 2, 2},
            new byte[] {2, 2, 2, 2, 2, 1},
            new byte[] {1, 2, 1, 2, 2, 3},
            new byte[] {1, 2, 1, 3, 2, 2},
            new byte[] {1, 3, 1, 2, 2, 2},
            new byte[] {1, 2, 2, 2, 1, 3},
            new byte[] {1, 2, 2, 3, 1, 2},
            new byte[] {1, 3, 2, 2, 1, 2},
            new byte[] {2, 2, 1, 2, 1, 3},
            new byte[] {2, 2, 1, 3, 1, 2},
            new byte[] {2, 3, 1, 2, 1, 2},
            new byte[] {1, 1, 2, 2, 3, 2},
            new byte[] {1, 2, 2, 1, 3, 2},
            new byte[] {1, 2, 2, 2, 3, 1},
            new byte[] {1, 1, 3, 2, 2, 2},
            new byte[] {1, 2, 3, 1, 2, 2},
            new byte[] {1, 2, 3, 2, 2, 1},
            new byte[] {2, 2, 3, 2, 1, 1},
            new byte[] {2, 2, 1, 1, 3, 2},
            new byte[] {2, 2, 1, 2, 3, 1},
            new byte[] {2, 1, 3, 2, 1, 2},
            new byte[] {2, 2, 3, 1, 1, 2},
            new byte[] {3, 1, 2, 1, 3, 1},
            new byte[] {3, 1, 1, 2, 2, 2},
            new byte[] {3, 2, 1, 1, 2, 2},
            new byte[] {3, 2, 1, 2, 2, 1},
            new byte[] {3, 1, 2, 2, 1, 2},
            new byte[] {3, 2, 2, 1, 1, 2},
            new byte[] {3, 2, 2, 2, 1, 1},
            new byte[] {2, 1, 2, 1, 2, 3},
            new byte[] {2, 1, 2, 3, 2, 1},
            new byte[] {2, 3, 2, 1, 2, 1},
            new byte[] {1, 1, 1, 3, 2, 3},
            new byte[] {1, 3, 1, 1, 2, 3},
            new byte[] {1, 3, 1, 3, 2, 1},
            new byte[] {1, 1, 2, 3, 1, 3},
            new byte[] {1, 3, 2, 1, 1, 3},
            new byte[] {1, 3, 2, 3, 1, 1},
            new byte[] {2, 1, 1, 3, 1, 3},
            new byte[] {2, 3, 1, 1, 1, 3},
            new byte[] {2, 3, 1, 3, 1, 1},
            new byte[] {1, 1, 2, 1, 3, 3},
            new byte[] {1, 1, 2, 3, 3, 1},
            new byte[] {1, 3, 2, 1, 3, 1},
            new byte[] {1, 1, 3, 1, 2, 3},
            new byte[] {1, 1, 3, 3, 2, 1},
            new byte[] {1, 3, 3, 1, 2, 1},
            new byte[] {3, 1, 3, 1, 2, 1},
            new byte[] {2, 1, 1, 3, 3, 1},
            new byte[] {2, 3, 1, 1, 3, 1},
            new byte[] {2, 1, 3, 1, 1, 3},
            new byte[] {2, 1, 3, 3, 1, 1},
            new byte[] {2, 1, 3, 1, 3, 1},
            new byte[] {3, 1, 1, 1, 2, 3},
            new byte[] {3, 1, 1, 3, 2, 1},
            new byte[] {3, 3, 1, 1, 2, 1},
            new byte[] {3, 1, 2, 1, 1, 3},
            new byte[] {3, 1, 2, 3, 1, 1},
            new byte[] {3, 3, 2, 1, 1, 1},
            new byte[] {3, 1, 4, 1, 1, 1},
            new byte[] {2, 2, 1, 4, 1, 1},
            new byte[] {4, 3, 1, 1, 1, 1},
            new byte[] {1, 1, 1, 2, 2, 4},
            new byte[] {1, 1, 1, 4, 2, 2},
            new byte[] {1, 2, 1, 1, 2, 4},
            new byte[] {1, 2, 1, 4, 2, 1},
            new byte[] {1, 4, 1, 1, 2, 2},
            new byte[] {1, 4, 1, 2, 2, 1},
            new byte[] {1, 1, 2, 2, 1, 4},
            new byte[] {1, 1, 2, 4, 1, 2},
            new byte[] {1, 2, 2, 1, 1, 4},
            new byte[] {1, 2, 2, 4, 1, 1},
            new byte[] {1, 4, 2, 1, 1, 2},
            new byte[] {1, 4, 2, 2, 1, 1},
            new byte[] {2, 4, 1, 2, 1, 1},
            new byte[] {2, 2, 1, 1, 1, 4},
            new byte[] {4, 1, 3, 1, 1, 1},
            new byte[] {2, 4, 1, 1, 1, 2},
            new byte[] {1, 3, 4, 1, 1, 1},
            new byte[] {1, 1, 1, 2, 4, 2},
            new byte[] {1, 2, 1, 1, 4, 2},
            new byte[] {1, 2, 1, 2, 4, 1},
            new byte[] {1, 1, 4, 2, 1, 2},
            new byte[] {1, 2, 4, 1, 1, 2},
            new byte[] {1, 2, 4, 2, 1, 1},
            new byte[] {4, 1, 1, 2, 1, 2},
            new byte[] {4, 2, 1, 1, 1, 2},
            new byte[] {4, 2, 1, 2, 1, 1},
            new byte[] {2, 1, 2, 1, 4, 1},
            new byte[] {2, 1, 4, 1, 2, 1},
            new byte[] {4, 1, 2, 1, 2, 1},
            new byte[] {1, 1, 1, 1, 4, 3},
            new byte[] {1, 1, 1, 3, 4, 1},
            new byte[] {1, 3, 1, 1, 4, 1},
            new byte[] {1, 1, 4, 1, 1, 3},
            new byte[] {1, 1, 4, 3, 1, 1},
            new byte[] {4, 1, 1, 1, 1, 3},
            new byte[] {4, 1, 1, 3, 1, 1},
            new byte[] {1, 1, 3, 1, 4, 1},
            new byte[] {1, 1, 4, 1, 3, 1},
            new byte[] {3, 1, 1, 1, 4, 1},
            new byte[] {4, 1, 1, 1, 3, 1},
            new byte[] {2, 1, 1, 4, 1, 2},
            new byte[] {2, 1, 1, 2, 1, 4},
            new byte[] {2, 1, 1, 2, 3, 2}
        };

        /// <summary>
        /// The stop bars.
        /// </summary>
        private static readonly byte[] _barsStop = { 2, 3, 3, 1, 1, 1, 2 };
        static Barcode128()
        {
            _ais[0] = 20;
            _ais[1] = 16;
            _ais[2] = 16;
            _ais[10] = -1;
            _ais[11] = 9;
            _ais[12] = 8;
            _ais[13] = 8;
            _ais[15] = 8;
            _ais[17] = 8;
            _ais[20] = 4;
            _ais[21] = -1;
            _ais[22] = -1;
            _ais[23] = -1;
            _ais[240] = -1;
            _ais[241] = -1;
            _ais[250] = -1;
            _ais[251] = -1;
            _ais[252] = -1;
            _ais[30] = -1;
            for (int k = 3100; k < 3700; ++k)
                _ais[k] = 10;
            _ais[37] = -1;
            for (int k = 3900; k < 3940; ++k)
                _ais[k] = -1;
            _ais[400] = -1;
            _ais[401] = -1;
            _ais[402] = 20;
            _ais[403] = -1;
            for (int k = 410; k < 416; ++k)
                _ais[k] = 16;
            _ais[420] = -1;
            _ais[421] = -1;
            _ais[422] = 6;
            _ais[423] = -1;
            _ais[424] = 6;
            _ais[425] = 6;
            _ais[426] = 6;
            _ais[7001] = 17;
            _ais[7002] = -1;
            for (int k = 7030; k < 7040; ++k)
                _ais[k] = -1;
            _ais[8001] = 18;
            _ais[8002] = -1;
            _ais[8003] = -1;
            _ais[8004] = -1;
            _ais[8005] = 10;
            _ais[8006] = 22;
            _ais[8007] = -1;
            _ais[8008] = -1;
            _ais[8018] = 22;
            _ais[8020] = -1;
            _ais[8100] = 10;
            _ais[8101] = 14;
            _ais[8102] = 6;
            for (int k = 90; k < 100; ++k)
                _ais[k] = -1;
        }

        /// <summary>
        /// Creates new Barcode128
        /// </summary>
        public Barcode128()
        {
            x = 0.8f;
            font = BaseFont.CreateFont("Helvetica", "winansi", false);
            size = 8;
            baseline = size;
            barHeight = size * 3;
            textAlignment = Element.ALIGN_CENTER;
            codeType = CODE128;
        }

        /// <summary>
        /// Gets the maximum area that the barcode and the text, if
        /// any, will occupy. The lower left corner is always (0, 0).
        /// </summary>
        /// <returns>the size the barcode occupies.</returns>
        public override Rectangle BarcodeSize
        {
            get
            {
                float fontX = 0;
                float fontY = 0;
                string fullCode;
                if (font != null)
                {
                    if (baseline > 0)
                        fontY = baseline - font.GetFontDescriptor(BaseFont.DESCENT, size);
                    else
                        fontY = -baseline + size;
                    if (codeType == CODE128_RAW)
                    {
                        int idx = code.IndexOf('\uffff');
                        if (idx < 0)
                            fullCode = "";
                        else
                            fullCode = code.Substring(idx + 1);
                    }
                    else if (codeType == CODE128_UCC)
                        fullCode = GetHumanReadableUccean(code);
                    else
                        fullCode = RemoveFnc1(code);
                    fontX = font.GetWidthPoint(altText != null ? altText : fullCode, size);
                }
                if (codeType == CODE128_RAW)
                {
                    int idx = code.IndexOf('\uffff');
                    if (idx >= 0)
                        fullCode = code.Substring(0, idx);
                    else
                        fullCode = code;
                }
                else
                {
                    fullCode = GetRawText(code, codeType == CODE128_UCC);
                }
                int len = fullCode.Length;
                float fullWidth = (len + 2) * 11 * x + 2 * x;
                fullWidth = Math.Max(fullWidth, fontX);
                float fullHeight = barHeight + fontY;
                return new Rectangle(fullWidth, fullHeight);
            }
        }

        /// <summary>
        /// Sets the code to generate. If it's an UCC code and starts with '(' it will
        /// be split by the AI. This code in UCC mode is valid:
        ///
        ///  (01)00000090311314(10)ABC123(15)060916
        /// </summary>
        public override string Code
        {
            set
            {
                string code = value;
                if (CodeType == CODE128_UCC && code.StartsWith("("))
                {
                    int idx = 0;
                    string ret = "";
                    while (idx >= 0)
                    {
                        int end = code.IndexOf(')', idx);
                        if (end < 0)
                            throw new ArgumentException("Badly formed UCC string: " + code);
                        string sai = code.Substring(idx + 1, end - (idx + 1));
                        if (sai.Length < 2)
                            throw new ArgumentException("AI too short: (" + sai + ")");
                        int ai = int.Parse(sai);
                        int len = _ais[ai];
                        if (len == 0)
                            throw new ArgumentException("AI not found: (" + sai + ")");
                        sai = ai.ToString();
                        if (sai.Length == 1)
                            sai = "0" + sai;
                        idx = code.IndexOf('(', end);
                        int next = (idx < 0 ? code.Length : idx);
                        ret += sai + code.Substring(end + 1, next - (end + 1));
                        if (len < 0)
                        {
                            if (idx >= 0)
                                ret += FNC1;
                        }
                        else if (next - end - 1 + sai.Length != len)
                            throw new ArgumentException("Invalid AI length: (" + sai + ")");
                    }
                    base.Code = ret;
                }
                else
                    base.Code = code;
            }
        }

        /// <summary>
        /// Generates the bars. The input has the actual barcodes, not
        /// the human readable text.
        /// </summary>
        /// <param name="text">the barcode</param>
        /// <returns>the bars</returns>
        public static byte[] GetBarsCode128Raw(string text)
        {
            int k;
            int idx = text.IndexOf('\uffff');
            if (idx >= 0)
                text = text.Substring(0, idx);
            int chk = text[0];
            for (k = 1; k < text.Length; ++k)
                chk += k * text[k];
            chk = chk % 103;
            text += (char)chk;
            byte[] bars = new byte[(text.Length + 1) * 6 + 7];
            for (k = 0; k < text.Length; ++k)
                Array.Copy(_bars[text[k]], 0, bars, k * 6, 6);
            Array.Copy(_barsStop, 0, bars, k * 6, 7);
            return bars;
        }

        /// <summary>
        /// Gets the human readable text of a sequence of AI.
        /// </summary>
        /// <param name="code">the text</param>
        /// <returns>the human readable text</returns>
        public static string GetHumanReadableUccean(string code)
        {
            StringBuilder buf = new StringBuilder();
            string fnc1 = FNC1.ToString();
            try
            {
                while (true)
                {
                    if (code.StartsWith(fnc1))
                    {
                        code = code.Substring(1);
                        continue;
                    }
                    int n = 0;
                    int idlen = 0;
                    for (int k = 2; k < 5; ++k)
                    {
                        if (code.Length < k)
                            break;
                        if ((n = _ais[int.Parse(code.Substring(0, k))]) != 0)
                        {
                            idlen = k;
                            break;
                        }
                    }
                    if (idlen == 0)
                        break;
                    buf.Append('(').Append(code.Substring(0, idlen)).Append(')');
                    code = code.Substring(idlen);
                    if (n > 0)
                    {
                        n -= idlen;
                        if (code.Length <= n)
                            break;
                        buf.Append(RemoveFnc1(code.Substring(0, n)));
                        code = code.Substring(n);
                    }
                    else
                    {
                        int idx = code.IndexOf(FNC1);
                        if (idx < 0)
                            break;
                        buf.Append(code.Substring(0, idx));
                        code = code.Substring(idx + 1);
                    }
                }
            }
            catch
            {
                //empty
            }
            buf.Append(RemoveFnc1(code));
            return buf.ToString();
        }

        /// <summary>
        /// Converts the human readable text to the characters needed to
        /// create a barcode. Some optimization is done to get the shortest code.
        /// the character FNC1 is added
        /// </summary>
        /// <param name="text">the text to convert</param>
        /// <param name="ucc"> true  if it is an UCC/EAN-128. In this case</param>
        /// <returns>the code ready to be fed to GetBarsCode128Raw()</returns>
        public static string GetRawText(string text, bool ucc)
        {
            string outs = "";
            int tLen = text.Length;
            if (tLen == 0)
            {
                outs += START_B;
                if (ucc)
                    outs += FNC1_INDEX;
                return outs;
            }
            int c = 0;
            for (int k = 0; k < tLen; ++k)
            {
                c = text[k];
                if (c > 127 && c != FNC1)
                    throw new ArgumentException("There are illegal characters for barcode 128 in '" + text + "'.");
            }
            c = text[0];
            char currentCode = START_B;
            int index = 0;
            if (IsNextDigits(text, index, 2))
            {
                currentCode = START_C;
                outs += currentCode;
                if (ucc)
                    outs += FNC1_INDEX;
                string out2 = GetPackedRawDigits(text, index, 2);
                index += out2[0];
                outs += out2.Substring(1);
            }
            else if (c < ' ')
            {
                currentCode = START_A;
                outs += currentCode;
                if (ucc)
                    outs += FNC1_INDEX;
                outs += (char)(c + 64);
                ++index;
            }
            else
            {
                outs += currentCode;
                if (ucc)
                    outs += FNC1_INDEX;
                if (c == FNC1)
                    outs += FNC1_INDEX;
                else
                    outs += (char)(c - ' ');
                ++index;
            }
            while (index < tLen)
            {
                switch (currentCode)
                {
                    case START_A:
                        {
                            if (IsNextDigits(text, index, 4))
                            {
                                currentCode = START_C;
                                outs += CODE_AB_TO_C;
                                string out2 = GetPackedRawDigits(text, index, 4);
                                index += out2[0];
                                outs += out2.Substring(1);
                            }
                            else
                            {
                                c = text[index++];
                                if (c == FNC1)
                                    outs += FNC1_INDEX;
                                else if (c > '_')
                                {
                                    currentCode = START_B;
                                    outs += CODE_AC_TO_B;
                                    outs += (char)(c - ' ');
                                }
                                else if (c < ' ')
                                    outs += (char)(c + 64);
                                else
                                    outs += (char)(c - ' ');
                            }
                        }
                        break;
                    case START_B:
                        {
                            if (IsNextDigits(text, index, 4))
                            {
                                currentCode = START_C;
                                outs += CODE_AB_TO_C;
                                string out2 = GetPackedRawDigits(text, index, 4);
                                index += out2[0];
                                outs += out2.Substring(1);
                            }
                            else
                            {
                                c = text[index++];
                                if (c == FNC1)
                                    outs += FNC1_INDEX;
                                else if (c < ' ')
                                {
                                    currentCode = START_A;
                                    outs += CODE_BC_TO_A;
                                    outs += (char)(c + 64);
                                }
                                else
                                {
                                    outs += (char)(c - ' ');
                                }
                            }
                        }
                        break;
                    case START_C:
                        {
                            if (IsNextDigits(text, index, 2))
                            {
                                string out2 = GetPackedRawDigits(text, index, 2);
                                index += out2[0];
                                outs += out2.Substring(1);
                            }
                            else
                            {
                                c = text[index++];
                                if (c == FNC1)
                                    outs += FNC1_INDEX;
                                else if (c < ' ')
                                {
                                    currentCode = START_A;
                                    outs += CODE_BC_TO_A;
                                    outs += (char)(c + 64);
                                }
                                else
                                {
                                    currentCode = START_B;
                                    outs += CODE_AC_TO_B;
                                    outs += (char)(c - ' ');
                                }
                            }
                        }
                        break;
                }
            }
            return outs;
        }

        /// <summary>
        /// Removes the FNC1 codes in the text.
        /// </summary>
        /// <param name="code">the text to clean</param>
        /// <returns>the cleaned text</returns>
        public static string RemoveFnc1(string code)
        {
            int len = code.Length;
            StringBuilder buf = new StringBuilder(len);
            for (int k = 0; k < len; ++k)
            {
                char c = code[k];
                if (c >= 32 && c <= 126)
                    buf.Append(c);
            }
            return buf.ToString();
        }
        public override System.Drawing.Image CreateDrawingImage(System.Drawing.Color foreground, System.Drawing.Color background)
        {
            string bCode;
            if (codeType == CODE128_RAW)
            {
                int idx = code.IndexOf('\uffff');
                if (idx >= 0)
                    bCode = code.Substring(0, idx);
                else
                    bCode = code;
            }
            else
            {
                bCode = GetRawText(code, codeType == CODE128_UCC);
            }
            int len = bCode.Length;
            int fullWidth = (len + 2) * 11 + 2;
            byte[] bars = GetBarsCode128Raw(bCode);
            int height = (int)barHeight;
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(fullWidth, height);
            for (int h = 0; h < height; ++h)
            {
                bool print = true;
                int ptr = 0;
                for (int k = 0; k < bars.Length; ++k)
                {
                    int w = bars[k];
                    System.Drawing.Color c = background;
                    if (print)
                        c = foreground;
                    print = !print;
                    for (int j = 0; j < w; ++j)
                        bmp.SetPixel(ptr++, h, c);
                }
            }
            return bmp;
        }

        /// <summary>
        /// Places the barcode in a  PdfContentByte . The
        /// barcode is always placed at coodinates (0, 0). Use the
        /// translation matrix to move it elsewhere.
        /// The bars and text are written in the following colors:
        ///
        ///
        ///    barColor
        ///    textColor
        ///   Result
        ///
        ///
        ///    null
        ///    null
        ///   bars and text painted with current fill color
        ///
        ///
        ///    barColor
        ///    null
        ///   bars and text painted with  barColor
        ///
        ///
        ///    null
        ///    textColor
        ///   bars painted with current color text painted with  textColor
        ///
        ///
        ///    barColor
        ///    textColor
        ///   bars painted with  barColor  text painted with  textColor
        ///
        ///
        /// </summary>
        /// <param name="cb">the  PdfContentByte  where the barcode will be placed</param>
        /// <param name="barColor">the color of the bars. It can be  null </param>
        /// <param name="textColor">the color of the text. It can be  null </param>
        /// <returns>the dimensions the barcode occupies</returns>
        public override Rectangle PlaceBarcode(PdfContentByte cb, BaseColor barColor, BaseColor textColor)
        {
            string fullCode;
            if (codeType == CODE128_RAW)
            {
                int idx = code.IndexOf('\uffff');
                if (idx < 0)
                    fullCode = "";
                else
                    fullCode = code.Substring(idx + 1);
            }
            else if (codeType == CODE128_UCC)
                fullCode = GetHumanReadableUccean(code);
            else
                fullCode = RemoveFnc1(code);
            float fontX = 0;
            if (font != null)
            {
                fontX = font.GetWidthPoint(fullCode = altText != null ? altText : fullCode, size);
            }
            string bCode;
            if (codeType == CODE128_RAW)
            {
                int idx = code.IndexOf('\uffff');
                if (idx >= 0)
                    bCode = code.Substring(0, idx);
                else
                    bCode = code;
            }
            else
            {
                bCode = GetRawText(code, codeType == CODE128_UCC);
            }
            int len = bCode.Length;
            float fullWidth = (len + 2) * 11 * x + 2 * x;
            float barStartX = 0;
            float textStartX = 0;
            switch (textAlignment)
            {
                case Element.ALIGN_LEFT:
                    break;
                case Element.ALIGN_RIGHT:
                    if (fontX > fullWidth)
                        barStartX = fontX - fullWidth;
                    else
                        textStartX = fullWidth - fontX;
                    break;
                default:
                    if (fontX > fullWidth)
                        barStartX = (fontX - fullWidth) / 2;
                    else
                        textStartX = (fullWidth - fontX) / 2;
                    break;
            }
            float barStartY = 0;
            float textStartY = 0;
            if (font != null)
            {
                if (baseline <= 0)
                    textStartY = barHeight - baseline;
                else
                {
                    textStartY = -font.GetFontDescriptor(BaseFont.DESCENT, size);
                    barStartY = textStartY + baseline;
                }
            }
            byte[] bars = GetBarsCode128Raw(bCode);
            bool print = true;
            if (barColor != null)
                cb.SetColorFill(barColor);
            for (int k = 0; k < bars.Length; ++k)
            {
                float w = bars[k] * x;
                if (print)
                    cb.Rectangle(barStartX, barStartY, w - inkSpreading, barHeight);
                print = !print;
                barStartX += w;
            }
            cb.Fill();
            if (font != null)
            {
                if (textColor != null)
                    cb.SetColorFill(textColor);
                cb.BeginText();
                cb.SetFontAndSize(font, size);
                cb.SetTextMatrix(textStartX, textStartY);
                cb.ShowText(fullCode);
                cb.EndText();
            }
            return BarcodeSize;
        }

        /// <summary>
        /// Packs the digits for charset C also considering FNC1. It assumes that all the parameters
        /// are valid.
        /// </summary>
        /// <param name="text">the text to pack</param>
        /// <param name="textIndex">where to pack from</param>
        /// <param name="numDigits">the number of digits to pack. It is always an even number</param>
        /// <returns>the packed digits, two digits per character</returns>
        internal static string GetPackedRawDigits(string text, int textIndex, int numDigits)
        {
            string outs = "";
            int start = textIndex;
            while (numDigits > 0)
            {
                if (text[textIndex] == FNC1)
                {
                    outs += FNC1_INDEX;
                    ++textIndex;
                    continue;
                }
                numDigits -= 2;
                int c1 = text[textIndex++] - '0';
                int c2 = text[textIndex++] - '0';
                outs += (char)(c1 * 10 + c2);
            }
            return (char)(textIndex - start) + outs;
        }

        /// <summary>
        /// Returns  true  if the next  numDigits
        /// starting from index  textIndex  are numeric skipping any FNC1.
        /// </summary>
        /// <param name="text">the text to check</param>
        /// <param name="textIndex">where to check from</param>
        /// <param name="numDigits">the number of digits to check</param>
        /// <returns>the check result</returns>
        internal static bool IsNextDigits(string text, int textIndex, int numDigits)
        {
            int len = text.Length;
            while (textIndex < len && numDigits > 0)
            {
                if (text[textIndex] == FNC1)
                {
                    ++textIndex;
                    continue;
                }
                int n = Math.Min(2, numDigits);
                if (textIndex + n > len)
                    return false;
                while (n-- > 0)
                {
                    char c = text[textIndex++];
                    if (c < '0' || c > '9')
                        return false;
                    --numDigits;
                }
            }
            return numDigits == 0;
        }
    }
}