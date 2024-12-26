using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prakt1ka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool IsValidInput(string input)
        {
            // Проверяем, что строка содержит только кириллические буквы
            return input.All(c => c >= 'А' && c <= 'Я');
        }
        private void genGraph_Click(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text.ToUpper(); // Приводим строку к верхнему регистру

            if (!IsValidInput(input))
            {
                MessageBox.Show("Введите строку, содержащую только кириллические буквы от А до Я.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int[] values = LetterConverter.ConvertStringToValues(input); // Преобразуем строку в массив чисел
            DrawGraph(values); // Рисуем график
        }
        private void DrawGraph(int[] values)
        {
            GraphCanvas.Children.Clear(); // Очистить Canvas перед отрисовкой

            if (values.Length == 0) return;

            double canvasHeight = GraphCanvas.ActualHeight; // Высота Canvas
            double canvasWidth = GraphCanvas.ActualWidth;   // Ширина Canvas

            double stepX = canvasWidth / (values.Length - 1); // Шаг по оси X

            // Диапазон значений по оси Y
            int minY = -16;
            int maxY = 15;
            double rangeY = maxY - minY;

            // Масштабирование для оси Y
            double scaleY = canvasHeight / rangeY;

            // Центр по оси Y
            double centerY = canvasHeight - (-minY * scaleY);

            // Рисуем оси
            DrawAxes(centerY,canvasWidth, canvasHeight, scaleY);

            Polyline polyline = new Polyline
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            for (int i = 0; i < values.Length; i++)
            {
                double x = i * stepX; // Координата по X
                double y = canvasHeight - (values[i] - minY) * scaleY; // Преобразуем значение в координату по Y
                y = Math.Min(Math.Max(0, y), canvasHeight); // Ограничиваем значение y в пределах канваса
                polyline.Points.Add(new Point(x, y));
            }

            GraphCanvas.Children.Add(polyline);
        }

        private void DrawAxes(double centerY, double canvasWidth, double canvasHeight, double scaleY)
        {
            // Горизонтальная ось X
            Line xAxis = new Line
            {
                X1 = 0,
                Y1 = centerY,
                X2 = canvasWidth,
                Y2 = centerY,
                Stroke = Brushes.Gray,
                StrokeThickness = 1
            };
            GraphCanvas.Children.Add(xAxis);

            // Вертикальная ось Y
            Line yAxis = new Line
            {
                X1 = canvasWidth / 2,
                Y1 = 0,
                X2 = canvasWidth / 2,
                Y2 = canvasHeight,
                Stroke = Brushes.Gray,
                StrokeThickness = 1
            };
            GraphCanvas.Children.Add(yAxis);

            // Метки на оси Y
            int minY = -16;
            int maxY = 15;

            for (int i = minY; i <= maxY; i++)
            {
                // Позиция по оси Y с учетом масштаба
                double yPosition = canvasHeight - (i - minY) * scaleY;

                // Если метка выходит за пределы Canvas, пропускаем её
                if (yPosition < 0 || yPosition > canvasHeight)
                    continue;

                // Линии делений
                Line tick = new Line
                {
                    X1 = canvasWidth / 2 - 5,
                    Y1 = yPosition,
                    X2 = canvasWidth / 2 + 5,
                    Y2 = yPosition,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                GraphCanvas.Children.Add(tick);

                // Текстовые метки
                TextBlock label = new TextBlock
                {
                    Text = i.ToString(),
                    Foreground = Brushes.Black
                };

                Canvas.SetLeft(label, canvasWidth / 2 + 10);
                Canvas.SetTop(label, yPosition - 10);

                GraphCanvas.Children.Add(label);
            }
        }

        public static class LetterConverter
        {
            public static int[] ConvertStringToValues(string input)
            {
                int[] result = new int[input.Length];

                for (int i = 0; i < input.Length; i++)
                {
                    result[i] = ConvertCharToValue(input[i]);
                }

                return result;
            }

            public static int ConvertCharToValue(char c)
            {
                int index = c - 'А';
                return index - 16;
            }
        }
    }
}
