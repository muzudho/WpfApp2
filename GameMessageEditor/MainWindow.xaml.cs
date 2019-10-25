namespace WpfApp2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using Microsoft.WindowsAPICodePack.Dialogs;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string FileName { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.FileName = string.Empty;
        }

        private void OpensButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                Title = "Title",
                // フォルダ選択ダイアログの場合は true
                IsFolderPicker = false,
                // ダイアログが表示されたときの初期ディレクトリを指定
                InitialDirectory = "適当なパス",

                // ユーザーが最近したアイテムの一覧を表示するかどうか
                AddToMostRecentlyUsedList = false,
                // ユーザーがフォルダやライブラリなどのファイルシステム以外の項目を選択できるようにするかどうか
                AllowNonFileSystemItems = false,
                // 最近使用されたフォルダが利用不可能な場合にデフォルトとして使用されるフォルダとパスを設定する
                DefaultDirectory = "適当なパス",
                // 存在するファイルのみ許可するかどうか
                EnsureFileExists = true,
                // 存在するパスのみ許可するかどうか
                EnsurePathExists = true,
                // 読み取り専用ファイルを許可するかどうか
                EnsureReadOnly = false,
                // 有効なファイル名のみ許可するかどうか（ファイル名を検証するかどうか）
                EnsureValidNames = true,
                // 複数選択を許可するかどうか
                Multiselect = false,
                // PC やネットワークなどの場所を表示するかどうか
                ShowPlacesList = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.FileName = dialog.FileName;
                var contents = File.ReadAllText(this.FileName);
                richTextBox.Document = AppliesStyle.Go(contents);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create a FlowDocument to contain content for the RichTextBox.
            FlowDocument myFlowDoc = new FlowDocument();
            {
                // Create a paragraph and add the Run and Bold to it.
                Paragraph myParagraph = new Paragraph();
                {
                    // Create a Run of plain text and some bold text.
                    var myRun = new Run("This is flow content and you can ");
                    myParagraph.Inlines.Add(myRun);

                    var myBold = new Bold(new Run("edit me!"));
                    myParagraph.Inlines.Add(myBold);

                    Hyperlink hyperl = new Hyperlink(new Run("Link Text."));
                    hyperl.NavigateUri = new Uri("http://search.msn.com");
                    myParagraph.Inlines.Add(hyperl);

                    myParagraph.Inlines.Add(new LineBreak());

                    var myItalic = new Italic(new Run("Italic"));
                    myParagraph.Inlines.Add(myItalic);

                    var underline = new Underline(new Run("Hello!"));
                    myParagraph.Inlines.Add(underline);

                    var highlight = new Run("Good morning!");
                    highlight.Foreground = Brushes.White;
                    highlight.Background = Brushes.Blue;
                    myParagraph.Inlines.Add(highlight);

                    var large = new Run("Yes.");
                    large.FontSize += 8.0;
                    myParagraph.Inlines.Add(large);
                }

                // Add the paragraph to the FlowDocument.
                myFlowDoc.Blocks.Add(myParagraph);
            }

            // Add initial content to the RichTextBox.
            richTextBox.Document = myFlowDoc;

            /*
            // 全文取得
            {
                string plainText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
                Trace.WriteLine(plainText);
            }
            */

            // なるべく等幅なフォント指定
            richTextBox.FontFamily = new FontFamily("ＭＳ ゴシック");
        }

        /// <summary>
        /// 上書き保存。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverwritesButton_Click_1(object sender, RoutedEventArgs e)
        {
            string plainText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            File.WriteAllText(this.FileName, plainText);
        }

        /// <summary>
        /// 最新に更新☆（＾～＾）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshesButton_Click(object sender, RoutedEventArgs e)
        {
            string plainText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            richTextBox.Document = AppliesStyle.Go(plainText);
        }
    }
}
