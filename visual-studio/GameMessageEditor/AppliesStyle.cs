namespace WpfApp2
{
    using System.Diagnostics;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    public static class AppliesStyle
    {
        /// <summary>
        /// 臙脂色。
        /// </summary>
        private static Brush SolidEnjiBrush = new SolidColorBrush(Color.FromRgb(179, 66, 74));

        /// <summary>
        /// 藍色。
        /// </summary>
        private static Brush SolidAiBrush = new SolidColorBrush(Color.FromRgb(35, 71, 148));

        /// <summary>
        /// 朱色。
        /// </summary>
        private static Brush SolidSyuBrush = new SolidColorBrush(Color.FromRgb(239, 69, 74));

        public static FlowDocument Go(string contents)
        {
            FlowDocument flowDoc = new FlowDocument();
            Paragraph paragraph = new Paragraph();

            // 改行は "\r"、 "\r\n"、 "\n" の３パターンがあるが、どれが入ってるか分からない。
            // そこで、"\r\n" は "\n" に変換し、残った "\r" を "\n" に変換する。
            contents = contents.Replace("\r\n", "\n");
            contents = contents.Replace('\r', '\n');
            var lines = contents.Split('\n');
            foreach (var line in lines)
            {
                if (line != string.Empty)
                {
                    Trace.WriteLine($"Line            | [{line}]");
                    if (line.StartsWith("$"))
                    {
                        var run = new Run(line);
                        run.Foreground = SolidEnjiBrush;
                        run.FontSize += 8;
                        paragraph.Inlines.Add(run);
                    }
                    else if (line.StartsWith("#"))
                    {
                        var run = new Run(line);
                        run.Foreground = Brushes.Green;
                        paragraph.Inlines.Add(run);
                    }
                    else if (line.StartsWith("&"))
                    {
                        var run = new Run(line);
                        run.Foreground = SolidAiBrush;
                        paragraph.Inlines.Add(run);
                    }
                    else
                    {
                        // { から } までに背景色を付ける。
                        // ただし、 {{ と }} は無視する。

                        var curr = 0;
                        while (curr < line.Length)
                        {
                            int next;

                            // ひら文
                            for (; ; )
                            {
                                next = line.IndexOf('{', curr);
                                if (next < 0)
                                {
                                    paragraph.Inlines.Add(new Run(line.Substring(curr)));
                                    goto line_loop;
                                }

                                if (next + 1 < line.Length && line[next + 1] == '{')
                                {
                                    paragraph.Inlines.Add(new Run(line.Substring(curr, next + 2 - curr)));
                                    curr = next + 2;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            // ひら文
                            var run = new Run(line.Substring(curr, next-curr));
                            paragraph.Inlines.Add(run);

                            // マーカー
                            curr = next;
                            for (; ; )
                            {
                                next = line.IndexOf('}', curr);
                                if (next < 0)
                                {
                                    // エラー
                                    run = new Run(line.Substring(curr));
                                    run.Background = SolidSyuBrush;
                                    paragraph.Inlines.Add(run);
                                    goto line_loop;
                                }

                                if (next + 1 < line.Length && line[next + 1] == '}')
                                {
                                    run = new Run(line.Substring(curr, next + 2 - curr));
                                    run.Background = Brushes.Yellow;
                                    paragraph.Inlines.Add(run);
                                    curr = next + 2;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            next += 1;
                            run = new Run(line.Substring(curr, next-curr));
                            run.Background = Brushes.Yellow;
                            paragraph.Inlines.Add(run);

                            // 次のループへ。
                            curr = next;
                        }
                    line_loop:
                        ;
                    }
                }

                // 改行。
                paragraph.Inlines.Add(new LineBreak());
            }
            flowDoc.Blocks.Add(paragraph);

            return flowDoc;
        }
    }
}
