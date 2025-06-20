using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        private int _iq = 0;
        private string _currentImagePath = "";

        public MainWindow()
        {
            InitializeComponent();
            UpdateIqUi();
        }

        private void OnIncreaseIQClick(object sender, RoutedEventArgs e)
        {
            _iq += 20;
            if (_iq > 200) _iq = 200;
            UpdateIqUi();
        }

        private void OnDoomClick(object sender, RoutedEventArgs e)
        {
            var doomWindow = new DoomWindow();
            doomWindow.IQRewarded += (s, reward) =>
            {
                _iq += reward;
                if (_iq > 200) _iq = 200;
                UpdateIqUi();
            };
            doomWindow.ShowDialog();
        }

        private void OnLogicSequenceClick(object sender, RoutedEventArgs e)
        {
            var answer = Microsoft.VisualBasic.Interaction.InputBox("Продолжите последовательность: 2, 4, 8, 16, ?", "Логика", "");
            if (answer.Trim() == "32")
            {
                MessageBox.Show("Верно! +10 к IQ");
                _iq += 10;
                UpdateIqUi();
            }
            else
                MessageBox.Show("Неверно! Правильный ответ: 32");
        }

        private void OnLogicOddOneClick(object sender, RoutedEventArgs e)
        {
            var answer = Microsoft.VisualBasic.Interaction.InputBox("Что лишнее: Кот, Собака, Волк, Слон?", "Логика", "");
            if (answer.ToLower().Contains("слон"))
            {
                MessageBox.Show("Верно! +10 к IQ");
                _iq += 10;
                UpdateIqUi();
            }
            else
                MessageBox.Show("Неверно! Правильный ответ: Слон");
        }

        private void OnMemorySequenceClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Запомните: 7 2 9 4", "Память");
            var answer = Microsoft.VisualBasic.Interaction.InputBox("Введите последовательность:", "Память", "");
            if (answer.Trim() == "7 2 9 4")
            {
                MessageBox.Show("Верно! +10 к IQ");
                _iq += 10;
                UpdateIqUi();
            }
            else
                MessageBox.Show("Неверно! Ответ: 7 2 9 4");
        }

        private void OnAnalogyClick(object sender, RoutedEventArgs e)
        {
            var answer = Microsoft.VisualBasic.Interaction.InputBox("Завод относится к рабочему, как школа к ...?", "Аналогии", "");
            if (answer.ToLower().Contains("ученик") || answer.ToLower().Contains("школьник"))
            {
                MessageBox.Show("Верно! +10 к IQ");
                _iq += 10;
                UpdateIqUi();
            }
            else
                MessageBox.Show("Неверно! Ответ: ученик/школьник");
        }

        private void OnArithmeticClick(object sender, RoutedEventArgs e)
        {
            var answer = Microsoft.VisualBasic.Interaction.InputBox("Вычислите: 15 + 28 = ?", "Арифметика", "");
            if (answer.Trim() == "43")
            {
                MessageBox.Show("Верно! +10 к IQ");
                _iq += 10;
                UpdateIqUi();
            }
            else
                MessageBox.Show("Неверно! Ответ: 43");
        }

        private void OnPairsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Мини-игра 'Найди пару' (заглушка). +10 к IQ");
            _iq += 10;
            UpdateIqUi();
        }

        private void OnShortMemoryClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Запомните: КОТ СТОЛ ЛИСА", "Кратковременная память");
            var answer = Microsoft.VisualBasic.Interaction.InputBox("Введите слова:", "Кратковременная память", "");
            if (answer.ToUpper().Contains("КОТ") && answer.ToUpper().Contains("СТОЛ") && answer.ToUpper().Contains("ЛИСА"))
            {
                MessageBox.Show("Верно! +10 к IQ");
                _iq += 10;
                UpdateIqUi();
            }
            else
                MessageBox.Show("Неверно! Ответ: КОТ СТОЛ ЛИСА");
        }

        private void OnSortNumbersClick(object sender, RoutedEventArgs e)
        {
            var answer = Microsoft.VisualBasic.Interaction.InputBox("Отсортируйте по возрастанию: 8 3 5 1", "Сортировка", "");
            if (answer.Trim() == "1 3 5 8")
            {
                MessageBox.Show("Верно! +10 к IQ");
                _iq += 10;
                UpdateIqUi();
            }
            else
                MessageBox.Show("Неверно! Ответ: 1 3 5 8");
        }

        private void UpdateIqUi()
        {
            IQprogress.Value = _iq;
            ProgressText.Text = $"{_iq} IQ";
            IQDescription.Text = GetIqDescription(_iq);
            UpdateComparisonImage(_iq);
        }

        private string GetIqDescription(int iq)
        {
            if (iq < 20) return "Абсолютный кретин";
            if (iq < 50) return "Кретин";
            if (iq < 80) return "Средний обыватель";
            if (iq < 120) return "Умный";
            if (iq < 160) return "Очень умный";
            if (iq < 200) return "Гений";
            return "Гений вселенной";
        }

        private void UpdateComparisonImage(int iq)
        {
            string newPath;
            if (iq <= 50)
                newPath = "Resources/Images/sonofwhore.jpg";
            else if (iq <= 80)
                newPath = "Resources/Images/zolo.png";
            else
                newPath = "Resources/Images/genius.jpg";

            if (_currentImagePath == newPath)
                return;

            _currentImagePath = newPath;
            var fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(180)));
            fadeOut.Completed += (s, e) =>
            {
                var img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri($"pack://application:,,,/{newPath}", UriKind.Absolute);
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                ComparisonImage.Source = img;

                var fadeIn = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(220)));
                ComparisonImage.BeginAnimation(Image.OpacityProperty, fadeIn);
            };
            ComparisonImage.BeginAnimation(Image.OpacityProperty, fadeOut);
        }
    }
}