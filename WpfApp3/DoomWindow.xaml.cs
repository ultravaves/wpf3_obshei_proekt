using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp3
{
    public partial class DoomWindow : Window
    {
        public event EventHandler<int> IQRewarded;

        private const int PlayerSize = 32;
        private const int EnemySize = 32;
        private const int BulletSize = 10;
        private const double PlayerSpeed = 10.0;
        private const double BulletSpeed = 15.0;
        private int enemySpeed = 5;

        private DispatcherTimer gameTimer;
        private Image player;
        private Image enemy;
        private Rectangle bullet;
        private bool isBulletActive = false;

        private double canvasWidth => GameCanvas.ActualWidth > 0 ? GameCanvas.ActualWidth : 800;
        private double canvasHeight => GameCanvas.ActualHeight > 0 ? GameCanvas.ActualHeight : 450;

        public DoomWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            GameCanvas.Children.Clear();

            // Игрок (иконка)
            player = CreateImage("Resources/ImagesDoom/brutal_doom_ico.ico", PlayerSize, PlayerSize);
            Canvas.SetLeft(player, canvasWidth / 2 - PlayerSize / 2);
            Canvas.SetTop(player, canvasHeight - PlayerSize * 2);
            GameCanvas.Children.Add(player);

            // Враг (иконка)
            enemy = CreateImage("Resources/ImagesDoom/icon_128x128.ico", EnemySize, EnemySize);
            Canvas.SetLeft(enemy, canvasWidth / 2 - EnemySize / 2);
            Canvas.SetTop(enemy, EnemySize);
            GameCanvas.Children.Add(enemy);

            isBulletActive = false;
            bullet = null;
            enemySpeed = 5;

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(50);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            this.KeyDown -= OnKeyDown;
            this.KeyDown += OnKeyDown;
            this.Focusable = true;
            this.Focus();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (enemy == null) return;
            MoveEnemy();
            if (isBulletActive)
            {
                MoveBullet();
                CheckCollision();
            }
        }

        private void MoveEnemy()
        {
            double currentLeft = Canvas.GetLeft(enemy);
            double proposedLeft = currentLeft + enemySpeed;

            if (proposedLeft < 0 || proposedLeft + EnemySize > canvasWidth)
                enemySpeed *= -1;
            Canvas.SetLeft(enemy, Canvas.GetLeft(enemy) + enemySpeed);
        }

        private void MoveBullet()
        {
            if (bullet == null) return;
            double currentTop = Canvas.GetTop(bullet);
            if (currentTop <= 0)
            {
                DeactivateBullet();
                return;
            }
            Canvas.SetTop(bullet, currentTop - BulletSpeed);
        }

        private void CheckCollision()
        {
            if (bullet == null || enemy == null) return;

            Rect bulletRect = new Rect(Canvas.GetLeft(bullet), Canvas.GetTop(bullet), BulletSize, BulletSize);
            Rect enemyRect = new Rect(Canvas.GetLeft(enemy), Canvas.GetTop(enemy), EnemySize, EnemySize);

            if (bulletRect.IntersectsWith(enemyRect))
            {
                GameCanvas.Children.Remove(enemy);
                GameCanvas.Children.Remove(bullet);
                isBulletActive = false;
                enemy = null;
                bullet = null;

                IQRewarded?.Invoke(this, 10);
                MessageBox.Show("Враг уничтожен! Вам начислено 10 очков.", "Победа", MessageBoxButton.OK, MessageBoxImage.Information);
                gameTimer.Stop();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (player == null) return;
            double currentLeft = Canvas.GetLeft(player);
            switch (e.Key)
            {
                case Key.Left:
                    if (currentLeft - PlayerSpeed >= 0)
                        Canvas.SetLeft(player, currentLeft - PlayerSpeed);
                    break;
                case Key.Right:
                    if (currentLeft + PlayerSize + PlayerSpeed <= canvasWidth)
                        Canvas.SetLeft(player, currentLeft + PlayerSpeed);
                    break;
                case Key.Space:
                    if (!isBulletActive)
                        FireBullet();
                    break;
            }
        }

        private void FireBullet()
        {
            bullet = new Rectangle
            {
                Width = BulletSize,
                Height = BulletSize,
                Fill = System.Windows.Media.Brushes.Yellow
            };
            double bulletLeft = Canvas.GetLeft(player) + PlayerSize / 2 - BulletSize / 2;
            double bulletTop = Canvas.GetTop(player) - BulletSize;
            Canvas.SetLeft(bullet, bulletLeft);
            Canvas.SetTop(bullet, bulletTop);
            GameCanvas.Children.Add(bullet);
            isBulletActive = true;
        }

        private void DeactivateBullet()
        {
            if (bullet != null)
            {
                GameCanvas.Children.Remove(bullet);
                bullet = null;
                isBulletActive = false;
            }
        }

        private Image CreateImage(string relativeUri, double width, double height)
        {
            var img = new Image
            {
                Width = width,
                Height = height,
                Source = new BitmapImage(new Uri($"pack://application:,,,/{relativeUri}", UriKind.Absolute))
            };
            return img;
        }
    }
}