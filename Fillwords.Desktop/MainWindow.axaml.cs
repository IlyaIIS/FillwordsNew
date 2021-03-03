using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Fillwords.Desktop
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataWorker.ReadWordsFromFile("words.txt");
            DataWorker.ReadUserScoreFromFile("users_score.txt");
            DataWorker.ReadSettingsFromFile("settings.txt");

            Printer.SetPrinterParams(((StackPanel)Content).Children,  this.Find<Window>("TheMainWindow"));

            if (DataWorker.WordsSet.AllWords.Length != 0)
                Printer.SetMainWindow(((StackPanel)Content).Children);
            else
                Printer.SetErrorWindow("Добавьте слова в словарь \"words.txt\" (сликом малое количество слов может не позволить сгенерировать поле)");
        }
    }
}
