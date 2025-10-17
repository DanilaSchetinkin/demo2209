using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using demo2209.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace demo2209
{
    public partial class MainWindow : Window
    {
        private Border[] puzzles;
        private int[] correctOrder = { 1, 2, 3, 4 };
        private int[] currentOrder = { 1, 2, 3, 4 };

        public MainWindow()
        {
            InitializeComponent();
            InitializePuzzleCaptcha();
        }

        private void InitializePuzzleCaptcha()
        {
            puzzles = new[] { Puzzle1, Puzzle2, Puzzle3, Puzzle4 };

            // Делаем пазлы перетаскиваемыми
            for (int i = 0; i < puzzles.Length; i++)
            {
                int index = i;
                puzzles[i].PointerPressed += (s, e) => OnPuzzlePressed(s, e, index);
            }

            // Перемешиваем пазлы
            ShufflePuzzles();
        }

        private void OnPuzzlePressed(object sender, PointerPressedEventArgs e, int index)
        {
            var puzzle = sender as Border;
            if (puzzle == null) return;

            // Находим индекс текущего положения пазла
            int currentIndex = Array.IndexOf(puzzles, puzzle);

            // Находим следующий индекс для перемещения (циклически)
            int nextIndex = (currentIndex + 1) % puzzles.Length;

            // Меняем местами пазлы
            SwapPuzzles(currentIndex, nextIndex);

            // Проверяем правильность порядка
            CheckPuzzleOrder();
        }

        private void SwapPuzzles(int index1, int index2)
        {
            // Сохраняем ссылки на пазлы
            var tempPuzzle = puzzles[index1];
            puzzles[index1] = puzzles[index2];
            puzzles[index2] = tempPuzzle;

            // Обновляем визуальное расположение
            Grid.SetColumn(puzzles[index1], index1);
            Grid.SetColumn(puzzles[index2], index2);

            // Обновляем текущий порядок
            var tempOrder = currentOrder[index1];
            currentOrder[index1] = currentOrder[index2];
            currentOrder[index2] = tempOrder;
        }

        private void ShufflePuzzles()
        {
            Random rnd = new Random();

            // Перемешиваем несколько раз для хорошего перемешивания
            for (int i = 0; i < 10; i++)
            {
                int index1 = rnd.Next(puzzles.Length);
                int index2 = rnd.Next(puzzles.Length);

                if (index1 != index2)
                {
                    SwapPuzzles(index1, index2);
                }
            }
        }

        private void CheckPuzzleOrder()
        {
            bool isCorrect = true;
            for (int i = 0; i < correctOrder.Length; i++)
            {
                if (currentOrder[i] != correctOrder[i])
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect)
            {
                CaptchaHint.Text = "✓ Пазл собран правильно!";
                CaptchaHint.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Green);
            }
            else
            {
                CaptchaHint.Text = "Перетащите пазлы в правильном порядке: 1-2-3-4";
                CaptchaHint.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Gray);
            }
        }

        private bool IsCaptchaSolved()
        {
            for (int i = 0; i < correctOrder.Length; i++)
            {
                if (currentOrder[i] != correctOrder[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void Button_Click(object? sender, RoutedEventArgs e)
        {
            string log = login.Text;
            string pass = password.Text;

            ErrorBox.Text = "";

            if (string.IsNullOrEmpty(log))
            {
                ShowError("Введите логин");
                return;
            }

            if (string.IsNullOrEmpty(pass))
            {
                ShowError("Введите пароль");
                return;
            }

            // Проверка капчи
            if (!IsCaptchaSolved())
            {
                ShowError("Соберите пазл правильно!");
                return;
            }

            
            using var context = new DanyaContext();

            var user = context.Employees
                .Include(emp => emp.PositionNavigation)
                .FirstOrDefault(emp => emp.Login == log && emp.Password == pass);

            var countId = context.LoginHistories.Count()+1;

            if (user == null)
            {
                var checkUser = context.Employees .FirstOrDefault(emp => emp.Login == log);
                if(checkUser == null)
                {
                    ShowError("Такого пользователя не существует!");
                    return;
                }
                else
                {
                    var newEnter = new LoginHistory()
                    {
                        Id = countId,
                        EmployeeId = checkUser.Id,
                        LoginTime = DateTime.Now,
                        LoginType = "Не успешно"
                    };

                    context.LoginHistories.Add(newEnter);
                    context.SaveChanges();

                    ShowError("Неверный пароль");
                    return;
                }

            }


            else
            {
                ShowError("Успешный вход!", false);
            }
            
          
            string roleName = user.PositionNavigation.RoleName;

            var newTime = new LoginHistory()
            {
                Id = countId,
                EmployeeId = user.Id,
                LoginTime = DateTime.Now,
                LoginType = "Успешно"
            };

            context.LoginHistories.Add(newTime);
            context.SaveChanges();


            var People = new PeopleWindow(user.Fio,roleName, user.Image);
            People.Show();
            this.Close();
        }

        private void ShowError(string message, bool isError = true)
        {
            ErrorBox.Text = message;
            if (isError)
            {
                ErrorBox.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red);
            }
            else
            {
                ErrorBox.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Green);
            }
        }
    }
}