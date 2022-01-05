using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using alfChessWorkout;

namespace alfChessWorkout
{
    public partial class Task36 : Form
    {   // картинка из PNG (все фигуры)
        public Image chessAllSprites;
        // размер доски и каждая клетка для фигур и ходов
        public int[,] chessboard = new int[8, 8]
        {   // 1 - белые, 2 - черные
            // 2 - ферзь, 1 - король, 5 - ладья, 4 - конь, 3 - слон, 6 - пешка 
            {0,0,25,0,0,0,0,0},
            {21,0,0,0,0,0,0,0},
            {14,26,0,0,0,0,26,0},
            {0,0,0,0,0,0,0,26},
            {0,16,0,0,23,0,0,16},
            {0,0,0,0,16,25,13,0},
            {15,0,0,0,0,0,11,0},
            {0,0,0,0,0,0,0,0},
        };
        // для просчёта возможных ходов (доп.массив кнопок)
        public Button[,] virtualButtons = new Button[8, 8];
        // переменная свидельствующая о начале партии
        public bool startPlayGame = false;
        // переменная в которой хранится нажатая клавиши (проверка) 
        public Button previousButton;
        // переменная где учитывается игрок, который ходит
        public bool playerIsMoving = false; // нет движения
        // переменная отвечает за текущего игрока
        public int currentPlayer; // за текущего игрока
        // переменная отвечает за того кто ходит первым
        public int whitePlayer = 0; // ходят белые фигуры 
        // количество шагов 
        public int userSteps = 0;
        // решение найдено за указаное количество ходов
        public int tempButton; // перемнная для кнопок фигур

        public Task36()
        {   // форма для 36 этюда
            InitializeComponent();
            // загрузка фигур из одного ресурса - спрайтами
            chessAllSprites = new Bitmap(alfChessWorkout.Properties.Resources.chess_3D); // файл chess_3D.png
            // указываю заголовок окна
            this.Text = " Этюд №36. Мат в один ход. Ход белых.";
            // загрузка фигур для этюда №36
            Init();
        }

        public void showTask36_click(object sender, EventArgs e)
        {
            if (pictureBox1.Visible == true)
            {
                pictureBox1.Visible = false; // скрываю рисунок (с задачей)
                buttonTask36.Text = "   ОБНОВИТЬ       ДАННЫЙ   ЭТЮД";
            }
            else
            {   // перезапуск этюда
                this.Controls.Clear();
                InitializeComponent();
                // загрузка фигур из одного ресурса - спрайтами
                chessAllSprites = new Bitmap(alfChessWorkout.Properties.Resources.chess_3D); // файл chess.png
                // загрузка фигур для этюда №36
                Init();
            }
        }

        public void Init()
        {   // расстановка фигур по умолчанию (для перезапуска партии)
            whitePlayer = 0;
            userSteps = 0;
            if (whitePlayer == 0) // и загружаю из ресурсов проекта)
            {
                chessboard = new int[8, 8]
                {   // 1 - белые, 2 - черные
                    // 2 - ферзь, 1 - король, 5 - ладья, 4 - конь, 3 - слон, 6 - пешка 
                    {0,0,25,0,0,0,0,0},
                    {21,0,0,0,0,0,0,0},
                    {14,26,0,0,0,0,26,0},
                    {0,0,0,0,0,0,0,26},
                    {0,16,0,0,23,0,0,16},
                    {0,0,0,0,16,25,13,0},
                    {15,0,0,0,0,0,11,0},
                    {0,0,0,0,0,0,0,0},
                };
            }
            currentPlayer = 1; // текущий игрок по умолчанию
            FigureChessBoard(); // фигуры на шахматной доске
        }

        public void FigureChessBoard()
        {   // создаю доску для игры (8 x 8)
            for (int iChess = 0; iChess < 8; iChess++)
            {
                for (int jChess = 0; jChess < 8; jChess++)
                {   // массив возможных ходов для каждой фигуры
                    virtualButtons[iChess, jChess] = new Button();
                    // заполняю кнопками (1 фигура - 1 кнопка)
                    Button buttonF = new Button();
                    // задаю размер кнопки (клетки с фигурой)
                    buttonF.Size = new Size(62, 62);
                    // указываю позицию на шахматной доске
                    buttonF.Location = new Point(jChess * 62 + 50, iChess * 62 + 50);
                    // рисую каждую кнопку из рисунка в ресурсах
                    // делю на 10 чтобы узнать чья это фигура - белые или черные
                    switch (chessboard[iChess, jChess] / 10)
                    {
                        case 1:
                            // беру только часть рисунка (одну фигуру, 70 x 70)
                            Image figura1 = new Bitmap(62, 62);
                            // каждую фигуру рисую из части рисунка
                            Graphics oneF = Graphics.FromImage(figura1);
                            // каждая фигура это квадрат из пикселей
                            oneF.DrawImage(chessAllSprites, new Rectangle(0, 0, 62, 62), 0 + 150 * (chessboard[iChess, jChess] % 10 - 1), 0, 150, 150, GraphicsUnit.Pixel);
                            // добавляю полученную картинку на кнопку
                            buttonF.BackgroundImage = figura1;
                            break;
                        case 2:
                            // беру только часть рисунка (одну фигуру, 70 x 70)
                            Image figura2 = new Bitmap(62, 62);
                            // каждую фигуру рисую из части рисунка
                            Graphics twoF = Graphics.FromImage(figura2);
                            // каждая фигура это квадрат из пикселей
                            twoF.DrawImage(chessAllSprites, new Rectangle(0, 0, 62, 62), 0 + 150 * (chessboard[iChess, jChess] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
                            // добавляю полученную картинку на кнопку
                            buttonF.BackgroundImage = figura2;
                            break;
                    }
                    buttonF.BackColor = Color.FromArgb(0, 116, 155); // темный фон кнопки-фигуры
                    if ((iChess + jChess) % 2 == 0)
                    {
                        buttonF.BackColor = Color.FromArgb(217, 217, 217); // светлый фон кнопки-фигуры    
                    }
                    // добавляю обработку нажатия (создаю ниже)
                    buttonF.Click += new EventHandler(PressOnChessFigure);
                    // и добавляю её (кнопку с изображением) на форму
                    this.Controls.Add(buttonF);
                    // кнопки возможных ходов
                    virtualButtons[iChess, jChess] = buttonF;
                }
            }
        }

        // обработка нажатий на фигуры-кнопки
        public void PressOnChessFigure(object sender, EventArgs e)
        {   // кнопка не нажата - фон стандартный
            if (previousButton != null)
            {
                if (previousButton.BackColor == Color.FromArgb(217, 217, 217))
                {   // был светлый - оставляю
                    previousButton.BackColor = Color.FromArgb(217, 217, 217);
                }
                else if (previousButton.BackColor == Color.FromArgb(0, 116, 155))
                {   // был тёмный - оставляю
                    previousButton.BackColor = Color.FromArgb(0, 116, 155);
                }
            }
            // переменная для нажатой кнопки
            Button pressedButton = sender as Button; // фон для возможных ходов
            Button pressedButtonTemp = sender as Button; // фон выбранной фигуры
            // чтобы не задействовать кнопки на которых нет фигур (проверка)
            if ((chessboard[pressedButton.Location.Y / 62, pressedButton.Location.X / 62] != 0) &&
               (chessboard[pressedButton.Location.Y / 62, pressedButton.Location.X / 62] / 10 == currentPlayer))
            {
                pressedButtonTemp.BackColor = Color.Red; // цвет при нажатии
                // разрешаю движение фигуры
                playerIsMoving = true; // разрешено движение фигуры
                // рисую возможные ходы (шаги)
                virtualStepsOnChessDesk(pressedButton.Location.Y / 62, pressedButton.Location.X / 62,
                   chessboard[pressedButton.Location.Y / 62, pressedButton.Location.X / 62]);
            }
            else
            {
                if (playerIsMoving) // если разрешено движение
                {   // для передачи между кнопками (ранее нажатая - сейчас нажата)
                    tempButton = chessboard[pressedButton.Location.Y / 62, pressedButton.Location.X / 62];
                    // нажатая берёт фон с предыдущей нажатой
                    chessboard[pressedButton.Location.Y / 62, pressedButton.Location.X / 62] = chessboard[previousButton.Location.Y / 62, previousButton.Location.X / 62];
                    // предыдущая берёт с переменной - tempButton
                    chessboard[previousButton.Location.Y / 62, previousButton.Location.X / 62] = tempButton;
                    // фон переходит от предыдущей кнопки
                    pressedButton.BackgroundImage = previousButton.BackgroundImage;
                    previousButton.BackgroundImage = null; // убираю
                    playerIsMoving = false; // запрещаю движение фигуры
                    userSteps++; // добавляю ход
                    DisableHideVirtualSteps(); // убираю виртуальные шаги
                    ChangePlayer(); // меняю игрока
                }
            }
            // обрабатываю текущий ход (привёл ли он к окончанию этюда)
            if ((userSteps == 1) && (pressedButton.Location.X == 174) && (pressedButton.Location.Y == 236) && (chessboard[2, 0] == 0))
            {
                labelTask36.ForeColor = Color.GreenYellow;
                labelTask36.Text = "Вы успешно решили Этюд №36.";
                MessageBox.Show("Вы успешно решили Этюд №36.", "Обновить Этюд №36", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Controls.Clear(); // чистка всех конролов
                InitializeComponent(); // загрузка по новой
                // загрузка фигур из одного ресурса - спрайтами
                chessAllSprites = new Bitmap(alfChessWorkout.Properties.Resources.chess_3D); // файл chess_3D.png
                Init(); // расстановка фигур для этюда
            }
            else if (userSteps > 1)
            {
                labelTask36.ForeColor = Color.OrangeRed;
                labelTask36.Text = "Решить за 1 ход не удалось.";
            }
            // передаю с кнопки на копку (рисунки фигур)
            if (previousButton != pressedButton)
            {
                previousButton = pressedButton;
            }
        }

        // обработка возможных ходов для каждой фигуры
        public void virtualStepsOnChessDesk(int iThisFigure, int jThisFigure, int thisFigure)
        {   // по цвету текущего игрока для пешки
            int pawnSteps = currentPlayer == 1 ? -1 : 1; // шаги вниз/вверх только для пешки
            switch (thisFigure % 10)
            {
                case 6: // возможные ходы для пешек
                    if (((iThisFigure == 1) && (currentPlayer == 1)) || ((iThisFigure == 6) && (currentPlayer == 2)))
                    {
                        if (InsideTheChessboard(iThisFigure + 1 * pawnSteps, jThisFigure))
                        {
                            if (chessboard[iThisFigure + 1 * pawnSteps, jThisFigure] == 0)
                            {
                                virtualButtons[iThisFigure + 1 * pawnSteps, jThisFigure].BackColor = Color.Yellow;
                                virtualButtons[iThisFigure + 1 * pawnSteps, jThisFigure].Enabled = true;
                                if (InsideTheChessboard(iThisFigure + 2 * pawnSteps, jThisFigure))
                                {
                                    if (chessboard[iThisFigure + 2 * pawnSteps, jThisFigure] == 0)
                                    {
                                        virtualButtons[iThisFigure + 2 * pawnSteps, jThisFigure].BackColor = Color.Yellow;
                                        virtualButtons[iThisFigure + 2 * pawnSteps, jThisFigure].Enabled = true;
                                    }
                                }
                            }
                        }
                        // есть ли фигуры справа чтобы их съесть
                        if (InsideTheChessboard(iThisFigure + 1 * pawnSteps, jThisFigure + 1))
                        {
                            if ((chessboard[iThisFigure + 1 * pawnSteps, jThisFigure + 1] != 0) &&
                                (chessboard[iThisFigure + 1 * pawnSteps, jThisFigure + 1] / 10 != currentPlayer))
                            {
                                virtualButtons[iThisFigure + 2 * pawnSteps, jThisFigure + 1].BackColor = Color.Yellow;
                                virtualButtons[iThisFigure + 2 * pawnSteps, jThisFigure + 1].Enabled = true;
                            }
                        }
                        // есть ли фигуры слева чтобы их съесть
                        if (InsideTheChessboard(iThisFigure + 1 * pawnSteps, jThisFigure - 1))
                        {
                            if ((chessboard[iThisFigure + 1 * pawnSteps, jThisFigure - 1] != 0) &&
                                (chessboard[iThisFigure + 1 * pawnSteps, jThisFigure - 1] / 10 != currentPlayer))
                            {
                                virtualButtons[iThisFigure + 2 * pawnSteps, jThisFigure - 1].BackColor = Color.Yellow;
                                virtualButtons[iThisFigure + 2 * pawnSteps, jThisFigure - 1].Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        if (InsideTheChessboard(iThisFigure + 1 * pawnSteps, jThisFigure))
                        {
                            if (chessboard[iThisFigure + 1 * pawnSteps, jThisFigure] == 0)
                            {
                                virtualButtons[iThisFigure + 1 * pawnSteps, jThisFigure].BackColor = Color.Yellow;
                                virtualButtons[iThisFigure + 1 * pawnSteps, jThisFigure].Enabled = true;
                                if (InsideTheChessboard(iThisFigure + 1 * pawnSteps, jThisFigure))
                                {
                                    if (chessboard[iThisFigure + 1 * pawnSteps, jThisFigure] == 0)
                                    {
                                        virtualButtons[iThisFigure + 1 * pawnSteps, jThisFigure].BackColor = Color.Yellow;
                                        virtualButtons[iThisFigure + 1 * pawnSteps, jThisFigure].Enabled = true;
                                    }
                                }
                            }
                        }
                        // есть ли фигуры справа чтобы их съесть
                        if (InsideTheChessboard(iThisFigure + 1 * pawnSteps, jThisFigure + 1))
                        {
                            if ((chessboard[iThisFigure + 1 * pawnSteps, jThisFigure + 1] != 0) &&
                                (chessboard[iThisFigure + 1 * pawnSteps, jThisFigure + 1] / 10 != currentPlayer))
                            {
                                virtualButtons[iThisFigure + 1 * pawnSteps, jThisFigure + 1].BackColor = Color.Yellow;
                                virtualButtons[iThisFigure + 1 * pawnSteps, jThisFigure + 1].Enabled = true;
                            }
                        }
                        // есть ли фигуры слева чтобы их съесть
                        if (InsideTheChessboard(iThisFigure + 1 * pawnSteps, jThisFigure - 1))
                        {
                            if ((chessboard[iThisFigure + 1 * pawnSteps, jThisFigure - 1] != 0) &&
                                (chessboard[iThisFigure + 1 * pawnSteps, jThisFigure - 1] / 10 != currentPlayer))
                            {
                                virtualButtons[iThisFigure + 1 * pawnSteps, jThisFigure - 1].BackColor = Color.Yellow;
                                virtualButtons[iThisFigure + 1 * pawnSteps, jThisFigure - 1].Enabled = true;
                            }
                        }
                    }

                    break;
                case 5:  // возможные ходы для ладьи
                    FunctionVirtualVerticalHorizontal(iThisFigure, jThisFigure);
                    break;
                case 4:  // возможные ходы для коня
                    FunctionHorseSteps(iThisFigure, jThisFigure);
                    break;
                case 3:  // возможные ходы для слона
                    FunctionDiagonalSteps(iThisFigure, jThisFigure);
                    break;
                case 2:  // возможные ходы для Королевы
                        FunctionVirtualVerticalHorizontal(iThisFigure, jThisFigure);
                        FunctionDiagonalSteps(iThisFigure, jThisFigure);
                    break;
                case 1:  // возможные ходы для Короля
                        FunctionVirtualVerticalHorizontal(iThisFigure, jThisFigure, true);
                        FunctionDiagonalSteps(iThisFigure, jThisFigure, true);
                    break;
            }
        }

        public void FunctionVirtualVerticalHorizontal(int iThisFigure, int jThisFigure, bool oneStep = false)
        {
            for (int iCurr = iThisFigure + 1; iCurr < 8; iCurr++)
            {   // возможные ходы вниз
                if (InsideTheChessboard(iCurr, jThisFigure))
                {   // проверка координат на доске
                    if (!CurrentPath(iCurr, jThisFigure))
                    {
                        break;
                    }
                }
                if (oneStep)
                    break;
            }
            for (int iCurr = iThisFigure - 1; iCurr >= 0; iCurr--)
            {   // возможные ходы верх
                if (InsideTheChessboard(iCurr, jThisFigure))
                {   // проверка координат на доске
                    if (!CurrentPath(iCurr, jThisFigure))
                    {
                        break;
                    }
                }
                if (oneStep)
                    break;
            }

            for (int jCurr = jThisFigure + 1; jCurr < 8; jCurr++)
            {   // возможные ходы вправо
                if (InsideTheChessboard(jThisFigure, jCurr))
                {   // проверка координат на доске
                    if (!CurrentPath(iThisFigure, jCurr))
                    {
                        break;
                    }
                }
                if (oneStep)
                    break;
            }
            for (int jCurr = jThisFigure - 1; jCurr >= 0; jCurr--)
            {   // возможные ходы влево
                if (InsideTheChessboard(jThisFigure, jCurr))
                {   // проверка координат на доске
                    if (!CurrentPath(iThisFigure, jCurr))
                    {
                        break;
                    }
                }
                if (oneStep)
                    break;
            }
        }

        // проверка пуста ли ячейка на доске
        public bool CurrentPath(int iCoord, int jCoord)
        {   // либо можно ходить либо нет
            if (chessboard[iCoord, jCoord] == 0)
            {   // значит сюда возможен ход
                virtualButtons[iCoord, jCoord].BackColor = Color.Yellow;
                virtualButtons[iCoord, jCoord].Enabled = true;
            }
            else
            {
                if (chessboard[iCoord, jCoord] / 10 != currentPlayer)
                {   // значит сюда возможен ход
                    virtualButtons[iCoord, jCoord].BackColor = Color.Yellow;
                    virtualButtons[iCoord, jCoord].Enabled = true;
                }
                return false;
            }
            return true;
        }

        // ходы по диагонали
        public void FunctionDiagonalSteps(int iThisFigure, int jThisFigure, bool oneStep = false)
        {   // по диагонали 
            int diagSteps = jThisFigure + 1; // шаги по диагонали вверх вправо
            for (int iD = iThisFigure - 1; iD >= 0; iD--)
            {   // проверка что шаги внутри доски
                if (InsideTheChessboard(iD, diagSteps))
                {   // возможен ли такой ход
                    if (!CurrentPath(iD, diagSteps))
                    {
                        break;
                    }   // чтоб не было выхода за доску
                    if (diagSteps < 7)
                        diagSteps++;
                    else
                        break;
                    if (oneStep)
                        break;
                }
            }
            diagSteps = jThisFigure - 1; // шаги по диагонали вверх влево
            for (int iD = iThisFigure - 1; iD >= 0; iD--)
            {
                if (InsideTheChessboard(iD, diagSteps))
                {
                    if (!CurrentPath(iD, diagSteps))
                    {
                        break;
                    }   // чтоб не было выхода за доску
                    if (diagSteps > 0)
                        diagSteps--;
                    else
                        break;
                    if (oneStep)
                        break;
                }
            }
            diagSteps = jThisFigure - 1; // шаги по диагонали вниз влево
            for (int iD = iThisFigure + 1; iD < 8; iD++)
            {
                if (InsideTheChessboard(iD, diagSteps))
                {
                    if (!CurrentPath(iD, diagSteps))
                    {
                        break;
                    }   // чтоб не было выхода за доску
                    if (diagSteps > 0)
                        diagSteps--;
                    else
                        break;
                    if (oneStep)
                        break;
                }
            }
            diagSteps = jThisFigure + 1; // шаги по диагонали вниз вправо
            for (int iD = iThisFigure + 1; iD < 8; iD++)
            {
                if (InsideTheChessboard(iD, diagSteps))
                {
                    if (!CurrentPath(iD, diagSteps))
                    {
                        break;
                    }   // чтоб не было выхода за доску
                    if (diagSteps < 7)
                        diagSteps++;
                    else
                        break;
                    if (oneStep)
                        break;
                }
            }
        }

        // ход конём
        public void FunctionHorseSteps(int iThisFigure, int jThisFigure)
        {   // возможные варианты, с проверкой по карте доски
            if (InsideTheChessboard(iThisFigure - 2, jThisFigure - 1))
            {   // 1 - ход конём
                CurrentPath(iThisFigure - 2, jThisFigure - 1);
            }
            if (InsideTheChessboard(iThisFigure - 2, jThisFigure + 1))
            {   // 2 - ход конём
                CurrentPath(iThisFigure - 2, jThisFigure + 1);
            }
            if (InsideTheChessboard(iThisFigure + 2, jThisFigure + 1))
            {   // 3 - ход конём
                CurrentPath(iThisFigure + 2, jThisFigure + 1);
            }
            if (InsideTheChessboard(iThisFigure + 2, jThisFigure - 1))
            {   // 4 - ход конём
                CurrentPath(iThisFigure + 2, jThisFigure - 1);
            }
            if (InsideTheChessboard(iThisFigure - 1, jThisFigure + 2))
            {   // 5 - ход конём
                CurrentPath(iThisFigure - 1, jThisFigure + 2);
            }
            if (InsideTheChessboard(iThisFigure + 1, jThisFigure + 2))
            {   // 6 - ход конём
                CurrentPath(iThisFigure + 1, jThisFigure + 2);
            }
            if (InsideTheChessboard(iThisFigure - 1, jThisFigure - 2))
            {   // 7 - ход конём
                CurrentPath(iThisFigure - 1, jThisFigure - 2);
            }
            if (InsideTheChessboard(iThisFigure + 1, jThisFigure - 2))
            {   // 8 - ход конём
                CurrentPath(iThisFigure + 1, jThisFigure - 2);
            }
        }

        // функция для смены текущего игрока
        public void ChangePlayer()
        {   // проверяю какой сейчас игрок ходит
            if (currentPlayer == 1)
            {   // тогда меняю
                currentPlayer = 2;
            }
            else
            {
                currentPlayer = 1;
            }
        }

        private void SelectedChangedColorPlayer(object sender, EventArgs e)
        {   // переключение игрока
            if (currentPlayer == 1) // белые
            {
                currentPlayer = 1;
            }
            else if (currentPlayer == 0) // черные
            {
                currentPlayer = 2;
            }
        }

        // находится ли указанное значение внутри шахматной доски
        public bool InsideTheChessboard(int iIns, int jIns)
        {   // в границах массива 8 на 8
            if (iIns >= 8 || jIns >= 8 || iIns < 0 || jIns < 0)
            {   // если вышел за доску
                return false;
            }
            return true; // всё хорошо
        }

        public void DisableHideVirtualSteps()
        {   // убираю вспомогательные шаги-подсказки
            for (int iStep = 0; iStep < 8; iStep++)
            {
                for (int jStep = 0; jStep < 8; jStep++)
                {
                    if ((iStep % 2 == 0) && ((jStep == 0) || (jStep == 2) || (jStep == 4) || (jStep == 6) || (jStep == 8)))
                    {   // светлый
                        virtualButtons[iStep, jStep].BackColor = Color.FromArgb(217, 217, 217);
                    }
                    else if ((iStep % 2 == 1) && ((jStep == 0) || (jStep == 2) || (jStep == 4) || (jStep == 6) || (jStep == 8)))
                    {   // тёмный
                        virtualButtons[iStep, jStep].BackColor = Color.FromArgb(0, 116, 155);
                    }
                    else if ((iStep % 2 == 0) && ((jStep == 1) || (jStep == 3) || (jStep == 5) || (jStep == 7)))
                    {   // тёмный
                        virtualButtons[iStep, jStep].BackColor = Color.FromArgb(0, 116, 155);
                    }
                    else if ((iStep % 2 == 1) && ((jStep == 1) || (jStep == 3) || (jStep == 5) || (jStep == 7)))
                    {   // светлый
                        virtualButtons[iStep, jStep].BackColor = Color.FromArgb(217, 217, 217);
                    }
                }
            }
        }

        // возможность хода
        public void forAllFigures_Deactivated()
        {
            for (int iChess = 0; iChess < 8; iChess++)
            {
                for (int jChess = 0; jChess < 8; jChess++)
                {   // отключаю возможность хода сюда
                    virtualButtons[iChess, jChess].Enabled = false;
                }
            }
        }
    // возможность хода
    public void forAllFigures_Activated()
    {
        for (int iChess = 0; iChess < 8; iChess++)
            {
                for (int jChess = 0; jChess < 8; jChess++)
                {   // включаю возможность хода сюда
                    virtualButtons[iChess, jChess].Enabled = true;
                }
            }
        }
    }
}
