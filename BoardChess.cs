using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using alfChessWorkout.Properties;

namespace alfChessWorkout
{   // шахматная доска для разбора примеров (этюдов)
    public partial class BoardChess : Form
    {   // переменные для взаимодействия между формамми
        public static int AllTask = 0; // номер этюда
        public static string allTask_label = ""; // пока не выбран этюд
        // картинка из PNG (все фигуры)
        public Image chessAllSprites;
        // размер доски и каждая клетка для фигур и ходов
        public int[,] chessboard = new int[8, 8]
        {   // 1 - белые, 2 - черные
            // 2 - ферзь, 1 - король, 5 - ладья, 4 - конь, 3 - слон, 6 - пешка 
            {15,14,13,12,11,13,14,15},
            {16,16,16,16,16,16,16,16},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {26,26,26,26,26,26,26,26},
            {25,24,23,22,21,23,24,25},
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
        // переменная отвечает за поворот доски
        public int whitePlayer = 0; // где белые фигуры сверху

        // главная форма
        public BoardChess()
        {
            InitializeComponent();
            // чтобы делать кнопки с прозрачным фоном
            // this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            // подгружаю иконку проекта для окна
            this.Icon = alfChessWorkout.Properties.Resources.ChessWorkout;
            // загрузка фигур из одного ресурса - спрайтами
            chessAllSprites = new Bitmap(alfChessWorkout.Properties.Resources.chess_one); // файл chess_one.png
            // указываю заголовок окна
            this.Text = " Программа обучения шахматам.";
            labelAllTask.Text = allTask_label; // пока нет этюдов
            // загрузка всех фигур
            Init();
        }

        public void Init()
        {   // расстановка фигур по умолчанию (для перезапуска партии)
            if(whitePlayer == 0) // и загружаю из ресурсов проекта)
            {
                chessboard = new int[8, 8]
                {   // 1 - белые, 2 - черные
                    // 2 - ферзь, 1 - король, 5 - ладья, 4 - конь, 3 - слон, 6 - пешка 
                    {15,14,13,12,11,13,14,15},
                    {16,16,16,16,16,16,16,16},
                    {0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0},
                    {26,26,26,26,26,26,26,26},
                    {25,24,23,22,21,23,24,25},
                };
            }
            else
            {
                chessboard = new int[8, 8]
                {   // 1 - белые, 2 - черные
                    // 2 - ферзь, 1 - король, 5 - ладья, 4 - конь, 3 - слон, 6 - пешка 
                    { 25,24,23,22,21,23,24,25},
                    { 26,26,26,26,26,26,26,26},
                    { 0,0,0,0,0,0,0,0},
                    { 0,0,0,0,0,0,0,0},
                    { 0,0,0,0,0,0,0,0},
                    { 0,0,0,0,0,0,0,0},
                    { 16,16,16,16,16,16,16,16},
                    { 15,14,13,12,11,13,14,15},
                };
            }
            
            comboBoxColorPlayer.Enabled = true; // разрешаю выбрать кто будет ходить первым
            comboBoxColorPlayer.SelectedIndex = 0; // выбраны белые

            currentPlayer = 1; // текущий игрок по умолчанию

            FigureChessBoard(); // фигуры на шахматной доске
            statusStripChess.ForeColor = Color.White; // белый цвет статуса
            labelPlayersChoice.Text = "Первыми ходят:";
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
                    buttonF.Size = new Size(70, 70);
                    // указываю позицию на шахматной доске
                    buttonF.Location = new Point(jChess * 70 + 50, iChess * 70 + 50);
                    // рисую каждую кнопку из рисунка в ресурсах
                    // делю на 10 чтобы узнать чья это фигура - белые или черные
                    switch (chessboard[iChess,jChess]/10)
                    {
                        case 1:
                            // беру только часть рисунка (одну фигуру, 70 x 70)
                            Image figura1 = new Bitmap(70, 70);
                            // каждую фигуру рисую из части рисунка
                            Graphics oneF = Graphics.FromImage(figura1);
                            // каждая фигура это квадрат из пикселей
                            oneF.DrawImage(chessAllSprites, new Rectangle(0, 0, 70, 70), 0 + 150 * (chessboard[iChess, jChess] % 10 - 1), 0, 150, 150, GraphicsUnit.Pixel);
                            // добавляю полученную картинку на кнопку
                            buttonF.BackgroundImage = figura1;
                            break;
                        case 2:
                            // беру только часть рисунка (одну фигуру, 70 x 70)
                            Image figura2 = new Bitmap(70, 70);
                            // каждую фигуру рисую из части рисунка
                            Graphics twoF = Graphics.FromImage(figura2);
                            // каждая фигура это квадрат из пикселей
                            twoF.DrawImage(chessAllSprites, new Rectangle(0, 0, 70, 70), 0 + 150 * (chessboard[iChess, jChess] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
                            // добавляю полученную картинку на кнопку
                            buttonF.BackgroundImage = figura2;
                            break;
                    }
                    buttonF.BackColor = Color.Black; // по умолчанию фон кнопки-фигуры
                    if ((iChess + jChess) % 2 == 0) 
                    {
                        buttonF.BackColor = Color.White; // белый фон кнопки-фигуры    
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
                if (previousButton.BackColor == Color.White)
                {   // был белый - оставляю
                    previousButton.BackColor = Color.White;
                }
                else if (previousButton.BackColor == Color.Black)
                {   // был чёрный - оставляю
                    previousButton.BackColor = Color.Black;
                }
            }
            // переменная для нажатой кнопки
            Button pressedButton = sender as Button; // фон для возможных ходов
            Button pressedButtonTemp = sender as Button; // фон выбранной фигуры
            // чтобы не задействовать кнопки на которых нет фигур (проверка)
            if ((chessboard[pressedButton.Location.Y / 70, pressedButton.Location.X / 70] != 0) &&
               (chessboard[pressedButton.Location.Y / 70, pressedButton.Location.X / 70] / 10 == currentPlayer))
            {
                pressedButtonTemp.BackColor = Color.Red; // цвет при нажатии
                // разрешаю движение фигуры
                playerIsMoving = true; // разрешено движение фигуры
                // рисую возможные ходы (шаги)
                virtualStepsOnChessDesk(pressedButton.Location.Y / 70, pressedButton.Location.X / 70, 
                    chessboard[pressedButton.Location.Y / 70, pressedButton.Location.X / 70]);
            }
            else
            {
                if (playerIsMoving) // если разрешено движение
                {   // для передачи между кнопками (ранее нажатая - сейчас нажата)
                    int tempButton = chessboard[pressedButton.Location.Y / 70, pressedButton.Location.X / 70];
                    // нажатая берёт фон с предыдущей нажатой
                    chessboard[pressedButton.Location.Y / 70, pressedButton.Location.X / 70] = chessboard[previousButton.Location.Y / 70, previousButton.Location.X / 70];
                    // предыдущая берёт с переменной - tempButton
                    chessboard[previousButton.Location.Y / 70, previousButton.Location.X / 70] = tempButton;
                    // фон переходит от предыдущей кнопки
                    pressedButton.BackgroundImage = previousButton.BackgroundImage;
                    previousButton.BackgroundImage = null; // убираю
                    playerIsMoving = false; // запрещаю движение фигуры
                    DisableHideVirtualSteps(); // убираю виртуальные шаги
                    ChangePlayer(); // меняю игрока
                    comboBoxColorPlayer.Enabled = false; // запрещаю менять игрока в игре
                }
            }
            // передаю с кнопки на копку
            previousButton = pressedButton;
        }

        // обработка возможных ходов для каждой фигуры
        public void virtualStepsOnChessDesk(int iThisFigure, int jThisFigure, int thisFigure)
        {   // по цвету текущего игрока для пешки
            int pawnSteps = currentPlayer == 1 ? 1 : -1; // шаги вниз/вверх только для пешки
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
                case 2:  // возможные ходы для короля
                    FunctionVirtualVerticalHorizontal(iThisFigure, jThisFigure);
                    FunctionDiagonalSteps(iThisFigure, jThisFigure);
                    break;
                case 1:  // возможные ходы для королевы
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
            {   // тогда меняю и статус
                currentPlayer = 2;
                toolStripOne.Text = "Ходят черные.";
            }
            else
            {
                currentPlayer = 1;
                toolStripOne.Text = "Ходят белые.";
            }
        }

        private void SelectedChangedColorPlayer(object sender, EventArgs e)
        {   // переключение игрока
            if (startPlayGame == false)
            {
                if (comboBoxColorPlayer.SelectedIndex == 0) // выбраны белые
                {
                    currentPlayer = 1;
                    toolStripOne.Text = "Ходят белые.";
                }
                else if ((comboBoxColorPlayer.SelectedIndex == 1) && (startPlayGame == false)) // выбраны черные
                {
                    currentPlayer = 2;
                    toolStripOne.Text = "Ходят черные.";
                }
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
                    {
                        virtualButtons[iStep, jStep].BackColor = Color.White;
                    }
                    else if ((iStep % 2 == 1) && ((jStep == 0) || (jStep == 2) || (jStep == 4) || (jStep == 6) || (jStep == 8)))
                    {
                        virtualButtons[iStep, jStep].BackColor = Color.Black;
                    }
                    else if ((iStep % 2 == 0) && ((jStep == 1) || (jStep == 3) || (jStep == 5) || (jStep == 7)))
                    {
                        virtualButtons[iStep, jStep].BackColor = Color.Black;
                    }
                    else if ((iStep % 2 == 1) && ((jStep == 1) || (jStep == 3) || (jStep == 5) || (jStep == 7)))
                    {
                        virtualButtons[iStep, jStep].BackColor = Color.White;
                    }
                }
            }
        }

        public void buttonResetGameAndRestart_Click(object sender, EventArgs e)
        {   // перезапуск партии (срос текущих позиций на доске)
            this.Controls.Clear(); // удаляю все контролы на доске
            // разрешаю выбрать кто будет ходить первым
            comboBoxColorPlayer.Enabled = true;
            // чтобы делать кнопки с прозрачным фоном
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            // подгружаю иконку проекта для окна
            this.Icon = alfChessWorkout.Properties.Resources.ChessWorkout;
            // загрузка фигур из одного ресурса - спрайтами (файл: chess_one.png)
            chessAllSprites = new Bitmap(alfChessWorkout.Properties.Resources.chess_one);
            // указываю заголовок окна
            this.Text = " Программа обучения шахматам";
            startPlayGame = false;
            toolStripOne.Enabled = true;
            toolStripOne.Visible = true;
            toolStripOne.Text = "Ходят белые.";
            labelPlayersChoice.Enabled = true;
            labelPlayersChoice.Visible = true;
            labelPlayersChoice.BackColor = Color.DarkBlue;
            labelPlayersChoice.Text = "Первыми ходят:";
            startPlayGame = false;
            comboBoxColorPlayer.Enabled = true;
            comboBoxColorPlayer.Visible = true;
            labelAllTask.Text = allTask_label;
            // загрузка всех фигур
            Init();
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

        // 1 этюд
        private void OneTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask01.BackColor = Color.MidnightBlue;
            this.buttonTask01.ForeColor = Color.Yellow;
        }

        private void OneTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask01.BackColor = Color.Yellow;
            this.buttonTask01.ForeColor = Color.MidnightBlue;
        }

        public void OneTask_Click(object sender, EventArgs e)
        {   // запуск формы с 1 этюдом
            this.buttonTask01.BackColor = Color.MidnightBlue;
            this.buttonTask01.ForeColor = Color.Yellow;
            Task01 newTask01 = new Task01();
            newTask01.ShowDialog();
        }

        // 2 этюд
        private void TwoTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask02.BackColor = Color.MidnightBlue;
            this.buttonTask02.ForeColor = Color.Yellow;
        }

        private void TwoTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask02.BackColor = Color.Yellow;
            this.buttonTask02.ForeColor = Color.MidnightBlue;
        }

        public void TwoTask_Click(object sender, EventArgs e)
        {   // запуск формы с 2 этюдом
            this.buttonTask02.BackColor = Color.MidnightBlue;
            this.buttonTask02.ForeColor = Color.Yellow;
            Task02 newTask02 = new Task02();
            newTask02.ShowDialog();
        }

        // 3 этюд
        private void ThreeTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask03.BackColor = Color.MidnightBlue;
            this.buttonTask03.ForeColor = Color.Yellow;
        }

        private void ThreeTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask03.BackColor = Color.Yellow;
            this.buttonTask03.ForeColor = Color.MidnightBlue;
        }

        public void ThreeTask_Click(object sender, EventArgs e)
        {   // запуск формы с 3 этюдом
            this.buttonTask03.BackColor = Color.MidnightBlue;
            this.buttonTask03.ForeColor = Color.Yellow;
            Task03 newTask03 = new Task03();
            newTask03.ShowDialog();
        }

        // 4 этюд
        private void FourTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask04.BackColor = Color.MidnightBlue;
            this.buttonTask04.ForeColor = Color.Yellow;
        }

        private void FourTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask04.BackColor = Color.Yellow;
            this.buttonTask04.ForeColor = Color.MidnightBlue;
        }

        public void FourTask_Click(object sender, EventArgs e)
        {   // запуск формы с 4 этюдом
            this.buttonTask04.BackColor = Color.MidnightBlue;
            this.buttonTask04.ForeColor = Color.Yellow;
            Task04 newTask04 = new Task04();
            newTask04.ShowDialog();
        }

        // 5 этюд
        private void FiveTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask05.BackColor = Color.MidnightBlue;
            this.buttonTask05.ForeColor = Color.Yellow;
        }

        private void FiveTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask05.BackColor = Color.Yellow;
            this.buttonTask05.ForeColor = Color.MidnightBlue;
        }

        public void FiveTask_Click(object sender, EventArgs e)
        {   // запуск формы с 5 этюдом
            this.buttonTask05.BackColor = Color.MidnightBlue;
            this.buttonTask05.ForeColor = Color.Yellow;
            Task05 newTask05 = new Task05();
            newTask05.ShowDialog();
        }

        // 6 этюд (ход черных)
        private void SixTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask06.BackColor = Color.MidnightBlue;
            this.buttonTask06.ForeColor = Color.Yellow;
        }

        private void SixTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask06.BackColor = Color.Yellow;
            this.buttonTask06.ForeColor = Color.MidnightBlue;
        }

        public void SixTask_Click(object sender, EventArgs e)
        {   // запуск формы с 6 этюдом
            this.buttonTask06.BackColor = Color.MidnightBlue;
            this.buttonTask06.ForeColor = Color.Yellow;
            Task06 newTask06 = new Task06();
            newTask06.ShowDialog();
        }

        // 7 этюд (ход белых)
        private void SevenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask07.BackColor = Color.MidnightBlue;
            this.buttonTask07.ForeColor = Color.Yellow;
        }

        private void SevenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask07.BackColor = Color.Yellow;
            this.buttonTask07.ForeColor = Color.MidnightBlue;
        }

        public void SevenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 7 этюдом
            this.buttonTask07.BackColor = Color.MidnightBlue;
            this.buttonTask07.ForeColor = Color.Yellow;
            Task07 newTask07 = new Task07();
            newTask07.ShowDialog();
        }

        // 8 этюд (ход белых)
        private void EightTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask08.BackColor = Color.MidnightBlue;
            this.buttonTask08.ForeColor = Color.Yellow;
        }

        private void EightTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask08.BackColor = Color.Yellow;
            this.buttonTask08.ForeColor = Color.MidnightBlue;
        }

        public void EightTask_Click(object sender, EventArgs e)
        {   // запуск формы с 8 этюдом
            this.buttonTask08.BackColor = Color.MidnightBlue;
            this.buttonTask08.ForeColor = Color.Yellow;
            Task08 newTask08 = new Task08();
            newTask08.ShowDialog();
        }

        // 9 этюд (ход белых)
        private void NineTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask09.BackColor = Color.MidnightBlue;
            this.buttonTask09.ForeColor = Color.Yellow;
        }

        private void NineTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask09.BackColor = Color.Yellow;
            this.buttonTask09.ForeColor = Color.MidnightBlue;
        }

        public void NineTask_Click(object sender, EventArgs e)
        {   // запуск формы с 9 этюдом
            this.buttonTask09.BackColor = Color.MidnightBlue;
            this.buttonTask09.ForeColor = Color.Yellow;
            Task09 newTask09 = new Task09();
            newTask09.ShowDialog();
        }

        // 10 этюд (ход белых) 3D
        private void TenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask10.BackColor = Color.MidnightBlue;
            this.buttonTask10.ForeColor = Color.Yellow;
        }

        private void TenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask10.BackColor = Color.Yellow;
            this.buttonTask10.ForeColor = Color.MidnightBlue;
        }

        public void TenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 10 этюдом
            this.buttonTask10.BackColor = Color.MidnightBlue;
            this.buttonTask10.ForeColor = Color.Yellow;
            Task10 newTask10 = new Task10();
            newTask10.ShowDialog();
        }

        // 11 этюд (ход белых) children
        private void ElevenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask11.BackColor = Color.MidnightBlue;
            this.buttonTask11.ForeColor = Color.Yellow;
        }

        private void ElevenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask11.BackColor = Color.Yellow;
            this.buttonTask11.ForeColor = Color.MidnightBlue;
        }

        public void ElevenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 11 этюдом
            this.buttonTask11.BackColor = Color.MidnightBlue;
            this.buttonTask11.ForeColor = Color.Yellow;
            Task11 newTask11 = new Task11();
            newTask11.ShowDialog();
        }

        // 12 этюд (ход белых) 
        private void TwelveTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask12.BackColor = Color.MidnightBlue;
            this.buttonTask12.ForeColor = Color.Yellow;
        }

        private void TwelveTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask12.BackColor = Color.Yellow;
            this.buttonTask12.ForeColor = Color.MidnightBlue;
        }

        public void TwelveTask_Click(object sender, EventArgs e)
        {   // запуск формы с 12 этюдом
            this.buttonTask12.BackColor = Color.MidnightBlue;
            this.buttonTask12.ForeColor = Color.Yellow;
            Task12 newTask12 = new Task12();
            newTask12.ShowDialog();
        }

        // 13 этюд (ход черных)
        private void ThirteenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask13.BackColor = Color.MidnightBlue;
            this.buttonTask13.ForeColor = Color.Yellow;
        }

        private void ThirteenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask13.BackColor = Color.Yellow;
            this.buttonTask13.ForeColor = Color.MidnightBlue;
        }

        public void ThirteenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 13 этюдом
            this.buttonTask13.BackColor = Color.MidnightBlue;
            this.buttonTask13.ForeColor = Color.Yellow;
            Task13 newTask13 = new Task13();
            newTask13.ShowDialog();
        }

        // 14 этюд (ход белых)
        private void FourteenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask14.BackColor = Color.MidnightBlue;
            this.buttonTask14.ForeColor = Color.Yellow;
        }

        private void FourteenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask14.BackColor = Color.Yellow;
            this.buttonTask14.ForeColor = Color.MidnightBlue;
        }

        public void FourteenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 14 этюдом
            this.buttonTask14.BackColor = Color.MidnightBlue;
            this.buttonTask14.ForeColor = Color.Yellow;
            Task14 newTask14 = new Task14();
            newTask14.ShowDialog();
        }

        // 15 этюд (ход белых)
        private void FifteenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask15.BackColor = Color.MidnightBlue;
            this.buttonTask15.ForeColor = Color.Yellow;
        }

        private void FifteenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask15.BackColor = Color.Yellow;
            this.buttonTask15.ForeColor = Color.MidnightBlue;
        }

        public void FifteenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 15 этюдом
            this.buttonTask15.BackColor = Color.MidnightBlue;
            this.buttonTask15.ForeColor = Color.Yellow;
            Task15 newTask15 = new Task15();
            newTask15.ShowDialog();
        }

        // 16 этюд (ход белых)
        private void SixteenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask16.BackColor = Color.MidnightBlue;
            this.buttonTask16.ForeColor = Color.Yellow;
        }

        private void SixteenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask16.BackColor = Color.Yellow;
            this.buttonTask16.ForeColor = Color.MidnightBlue;
        }

        public void SixteenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 16 этюдом
            this.buttonTask16.BackColor = Color.MidnightBlue;
            this.buttonTask16.ForeColor = Color.Yellow;
            Task16 newTask16 = new Task16();
            newTask16.ShowDialog();
        }

        // 17 этюд (ход белых)
        private void SeventeenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask17.BackColor = Color.MidnightBlue;
            this.buttonTask17.ForeColor = Color.Yellow;
        }

        private void SeventeenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask17.BackColor = Color.Yellow;
            this.buttonTask17.ForeColor = Color.MidnightBlue;
        }

        public void SeventeenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 17 этюдом
            this.buttonTask17.BackColor = Color.MidnightBlue;
            this.buttonTask17.ForeColor = Color.Yellow;
            Task17 newTask17 = new Task17();
            newTask17.ShowDialog();
        }

        // 18 этюд (ход белых)
        private void EighteenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask18.BackColor = Color.MidnightBlue;
            this.buttonTask18.ForeColor = Color.Yellow;
        }

        private void EighteenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask18.BackColor = Color.Yellow;
            this.buttonTask18.ForeColor = Color.MidnightBlue;
        }

        public void EighteenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 18 этюдом
            this.buttonTask18.BackColor = Color.MidnightBlue;
            this.buttonTask18.ForeColor = Color.Yellow;
            Task18 newTask18 = new Task18();
            newTask18.ShowDialog();
        }

        // 19 этюд (ход белых)
        private void NineteenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask19.BackColor = Color.MidnightBlue;
            this.buttonTask19.ForeColor = Color.Yellow;
        }

        private void NineteenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask19.BackColor = Color.Yellow;
            this.buttonTask19.ForeColor = Color.MidnightBlue;
        }

        public void NineteenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 19 этюдом
            this.buttonTask19.BackColor = Color.MidnightBlue;
            this.buttonTask19.ForeColor = Color.Yellow;
            Task19 newTask19 = new Task19();
            newTask19.ShowDialog();
        }

        // 20 этюд (ход белых)
        private void TwentyTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask20.BackColor = Color.MidnightBlue;
            this.buttonTask20.ForeColor = Color.Yellow;
        }

        private void TwentyTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask20.BackColor = Color.Yellow;
            this.buttonTask20.ForeColor = Color.MidnightBlue;
        }

        public void TwentyTask_Click(object sender, EventArgs e)
        {   // запуск формы с 20 этюдом
            this.buttonTask20.BackColor = Color.MidnightBlue;
            this.buttonTask20.ForeColor = Color.Yellow;
            Task20 newTask20 = new Task20();
            newTask20.ShowDialog();
        }

        // 21 этюд (ход белых)
        private void TwentyOneTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask21.BackColor = Color.MidnightBlue;
            this.buttonTask21.ForeColor = Color.Yellow;
        }

        private void TwentyOneTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask21.BackColor = Color.Yellow;
            this.buttonTask21.ForeColor = Color.MidnightBlue;
        }

        public void TwentyOneTask_Click(object sender, EventArgs e)
        {   // запуск формы с 21 этюдом
            this.buttonTask21.BackColor = Color.MidnightBlue;
            this.buttonTask21.ForeColor = Color.Yellow;
            Task21 newTask21 = new Task21();
            newTask21.ShowDialog();
        }

        // 22 этюд (ход белых)
        private void TwentyTwoTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask22.BackColor = Color.MidnightBlue;
            this.buttonTask22.ForeColor = Color.Yellow;
        }

        private void TwentyTwoTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask22.BackColor = Color.Yellow;
            this.buttonTask22.ForeColor = Color.MidnightBlue;
        }

        public void TwentyTwoTask_Click(object sender, EventArgs e)
        {   // запуск формы с 22 этюдом
            this.buttonTask22.BackColor = Color.MidnightBlue;
            this.buttonTask22.ForeColor = Color.Yellow;
            Task22 newTask22 = new Task22();
            newTask22.ShowDialog();
        }

        // 23 этюд (ход белых) 
        private void TwentyThreeTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask23.BackColor = Color.MidnightBlue;
            this.buttonTask23.ForeColor = Color.Yellow;
        }

        private void TwentyThreeTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask23.BackColor = Color.Yellow;
            this.buttonTask23.ForeColor = Color.MidnightBlue;
        }

        public void TwentyThreeTask_Click(object sender, EventArgs e)
        {   // запуск формы с 23 этюдом
            this.buttonTask23.BackColor = Color.MidnightBlue;
            this.buttonTask23.ForeColor = Color.Yellow;
            Task23 newTask23 = new Task23();
            newTask23.ShowDialog();
        }

        // 24 этюд (ход белых) 
        private void TwentyFourTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask24.BackColor = Color.MidnightBlue;
            this.buttonTask24.ForeColor = Color.Yellow;
        }

        private void TwentyFourTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask24.BackColor = Color.Yellow;
            this.buttonTask24.ForeColor = Color.MidnightBlue;
        }

        public void TwentyFourTask_Click(object sender, EventArgs e)
        {   // запуск формы с 24 этюдом
            this.buttonTask24.BackColor = Color.MidnightBlue;
            this.buttonTask24.ForeColor = Color.Yellow;
            Task24 newTask24 = new Task24();
            newTask24.ShowDialog();
        }

        // 25 этюд (ход белых)  
        private void TwentyFiveTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask25.BackColor = Color.MidnightBlue;
            this.buttonTask25.ForeColor = Color.Yellow;
        }

        private void TwentyFiveTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask25.BackColor = Color.Yellow;
            this.buttonTask25.ForeColor = Color.MidnightBlue;
        }

        public void TwentyFiveTask_Click(object sender, EventArgs e)
        {   // запуск формы с 25 этюдом
            this.buttonTask25.BackColor = Color.MidnightBlue;
            this.buttonTask25.ForeColor = Color.Yellow;
            Task25 newTask25 = new Task25();
            newTask25.ShowDialog();
        }

        // 26 этюд (ход белых)  
        private void TwentySixTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask26.BackColor = Color.MidnightBlue;
            this.buttonTask26.ForeColor = Color.Yellow;
        }

        private void TwentySixTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask26.BackColor = Color.Yellow;
            this.buttonTask26.ForeColor = Color.MidnightBlue;
        }

        public void TwentySixTask_Click(object sender, EventArgs e)
        {   // запуск формы с 26 этюдом
            this.buttonTask26.BackColor = Color.MidnightBlue;
            this.buttonTask26.ForeColor = Color.Yellow;
            Task26 newTask26 = new Task26();
            newTask26.ShowDialog();
        }

        // 27 этюд (ход белых)  
        private void TwentySevenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask27.BackColor = Color.MidnightBlue;
            this.buttonTask27.ForeColor = Color.Yellow;
        }

        private void TwentySevenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask27.BackColor = Color.Yellow;
            this.buttonTask27.ForeColor = Color.MidnightBlue;
        }

        public void TwentySevenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 27 этюдом
            this.buttonTask27.BackColor = Color.MidnightBlue;
            this.buttonTask27.ForeColor = Color.Yellow;
            Task27 newTask27 = new Task27();
            newTask27.ShowDialog();
        }

        // 28 этюд (ход белых)
        private void TwentyEightTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask28.BackColor = Color.MidnightBlue;
            this.buttonTask28.ForeColor = Color.Yellow;
        }

        private void TwentyEightTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask28.BackColor = Color.Yellow;
            this.buttonTask28.ForeColor = Color.MidnightBlue;
        }

        public void TwentyEightTask_Click(object sender, EventArgs e)
        {   // запуск формы с 28 этюдом
            this.buttonTask28.BackColor = Color.MidnightBlue;
            this.buttonTask28.ForeColor = Color.Yellow;
            Task28 newTask28 = new Task28();
            newTask28.ShowDialog();
        }

        // 29 этюд (ход белых) 
        private void TwentyNineTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask29.BackColor = Color.MidnightBlue;
            this.buttonTask29.ForeColor = Color.Yellow;
        }

        private void TwentyNineTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask29.BackColor = Color.Yellow;
            this.buttonTask29.ForeColor = Color.MidnightBlue;
        }

        public void TwentyNineTask_Click(object sender, EventArgs e)
        {   // запуск формы с 29 этюдом
            this.buttonTask29.BackColor = Color.MidnightBlue;
            this.buttonTask29.ForeColor = Color.Yellow;
            Task29 newTask29 = new Task29();
            newTask29.ShowDialog();
        }

        // 30 этюд (ход белых) 
        private void ThirtyTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask30.BackColor = Color.MidnightBlue;
            this.buttonTask30.ForeColor = Color.Yellow;
        }

        private void ThirtyTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask30.BackColor = Color.Yellow;
            this.buttonTask30.ForeColor = Color.MidnightBlue;
        }

        public void ThirtyTask_Click(object sender, EventArgs e)
        {   // запуск формы с 30 этюдом
            this.buttonTask30.BackColor = Color.MidnightBlue;
            this.buttonTask30.ForeColor = Color.Yellow;
            Task30 newTask30 = new Task30();
            newTask30.ShowDialog();
        }

        // 31 этюд (ход белых) 
        private void ThirtyOneTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask31.BackColor = Color.MidnightBlue;
            this.buttonTask31.ForeColor = Color.Yellow;
        }

        private void ThirtyOneTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask31.BackColor = Color.Yellow;
            this.buttonTask31.ForeColor = Color.MidnightBlue;
        }

        public void ThirtyOneTask_Click(object sender, EventArgs e)
        {   // запуск формы с 31 этюдом
            this.buttonTask31.BackColor = Color.MidnightBlue;
            this.buttonTask31.ForeColor = Color.Yellow;
            Task31 newTask31 = new Task31();
            newTask31.ShowDialog();
        }

        // 32 этюд (ход белых) 
        private void ThirtyTwoTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask32.BackColor = Color.MidnightBlue;
            this.buttonTask32.ForeColor = Color.Yellow;
        }

        private void ThirtyTwoTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask32.BackColor = Color.Yellow;
            this.buttonTask32.ForeColor = Color.MidnightBlue;
        }

        public void ThirtyTwoTask_Click(object sender, EventArgs e)
        {   // запуск формы с 32 этюдом
            this.buttonTask32.BackColor = Color.MidnightBlue;
            this.buttonTask32.ForeColor = Color.Yellow;
            Task32 newTask32 = new Task32();
            newTask32.ShowDialog();
        }

        // 33 этюд (ход белых) 
        private void ThirtyThreeTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask33.BackColor = Color.MidnightBlue;
            this.buttonTask33.ForeColor = Color.Yellow;
        }

        private void ThirtyThreeTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask33.BackColor = Color.Yellow;
            this.buttonTask33.ForeColor = Color.MidnightBlue;
        }

        public void ThirtyThreeTask_Click(object sender, EventArgs e)
        {   // запуск формы с 33 этюдом
            this.buttonTask33.BackColor = Color.MidnightBlue;
            this.buttonTask33.ForeColor = Color.Yellow;
            Task33 newTask33 = new Task33();
            newTask33.ShowDialog();
        }

        // 34 этюд (ход белых)
        private void ThirtyFourTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask34.BackColor = Color.MidnightBlue;
            this.buttonTask34.ForeColor = Color.Yellow;
        }

        private void ThirtyFourTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask34.BackColor = Color.Yellow;
            this.buttonTask34.ForeColor = Color.MidnightBlue;
        }

        public void ThirtyFourTask_Click(object sender, EventArgs e)
        {   // запуск формы с 34 этюдом
            this.buttonTask34.BackColor = Color.MidnightBlue;
            this.buttonTask34.ForeColor = Color.Yellow;
            Task34 newTask34 = new Task34();
            newTask34.ShowDialog();
        }

        // 35 этюд (ход белых)
        private void ThirtyFiveTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask35.BackColor = Color.MidnightBlue;
            this.buttonTask35.ForeColor = Color.Yellow;
        }

        private void ThirtyFiveTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask35.BackColor = Color.Yellow;
            this.buttonTask35.ForeColor = Color.MidnightBlue;
        }

        public void ThirtyFiveTask_Click(object sender, EventArgs e)
        {   // запуск формы с 35 этюдом
            this.buttonTask35.BackColor = Color.MidnightBlue;
            this.buttonTask35.ForeColor = Color.Yellow;
            Task35 newTask35 = new Task35();
            newTask35.ShowDialog();
        }

        // 36 этюд (ход белых)
        private void ThirtySixTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask36.BackColor = Color.MidnightBlue;
            this.buttonTask36.ForeColor = Color.Yellow;
        }

        private void ThirtySixTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask36.BackColor = Color.Yellow;
            this.buttonTask36.ForeColor = Color.MidnightBlue;
        }

        public void ThirtySixTask_Click(object sender, EventArgs e)
        {   // запуск формы с 36 этюдом
            this.buttonTask36.BackColor = Color.MidnightBlue;
            this.buttonTask36.ForeColor = Color.Yellow;
            Task36 newTask36 = new Task36();
            newTask36.ShowDialog();
        }

        // 37 этюд (ход белых) 
        private void ThirtySevenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask37.BackColor = Color.MidnightBlue;
            this.buttonTask37.ForeColor = Color.Yellow;
        }

        private void ThirtySevenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask37.BackColor = Color.Yellow;
            this.buttonTask37.ForeColor = Color.MidnightBlue;
        }

        public void ThirtySevenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 37 этюдом
            this.buttonTask37.BackColor = Color.MidnightBlue;
            this.buttonTask37.ForeColor = Color.Yellow;
            Task37 newTask37 = new Task37();
            newTask37.ShowDialog();
        }

        // 38 этюд (ход белых)  
        private void ThirtyEightTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask38.BackColor = Color.MidnightBlue;
            this.buttonTask38.ForeColor = Color.Yellow;
        }

        private void ThirtyEightTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask38.BackColor = Color.Yellow;
            this.buttonTask38.ForeColor = Color.MidnightBlue;
        }

        public void ThirtyEightTask_Click(object sender, EventArgs e)
        {   // запуск формы с 38 этюдом
            this.buttonTask38.BackColor = Color.MidnightBlue;
            this.buttonTask38.ForeColor = Color.Yellow;
            Task38 newTask38 = new Task38();
            newTask38.ShowDialog();
        }

        // 39 этюд (ход белых)  
        private void ThirtyNineTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask39.BackColor = Color.MidnightBlue;
            this.buttonTask39.ForeColor = Color.Yellow;
        }

        private void ThirtyNineTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask39.BackColor = Color.Yellow;
            this.buttonTask39.ForeColor = Color.MidnightBlue;
        }

        public void ThirtyNineTask_Click(object sender, EventArgs e)
        {   // запуск формы с 39 этюдом
            this.buttonTask39.BackColor = Color.MidnightBlue;
            this.buttonTask39.ForeColor = Color.Yellow;
            Task39 newTask39 = new Task39();
            newTask39.ShowDialog();
        }

        // 40 этюд (ход черных) 
        private void FortyTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask40.BackColor = Color.MidnightBlue;
            this.buttonTask40.ForeColor = Color.Yellow;
        }

        private void FortyTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask40.BackColor = Color.Yellow;
            this.buttonTask40.ForeColor = Color.MidnightBlue;
        }

        public void FortyTask_Click(object sender, EventArgs e)
        {   // запуск формы с 40 этюдом
            this.buttonTask40.BackColor = Color.MidnightBlue;
            this.buttonTask40.ForeColor = Color.Yellow;
            Task40 newTask40 = new Task40();
            newTask40.ShowDialog();
        }

        // 41 этюд (ход черных) 
        private void FortyOneTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask41.BackColor = Color.MidnightBlue;
            this.buttonTask41.ForeColor = Color.Yellow;
        }

        private void FortyOneTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask41.BackColor = Color.Yellow;
            this.buttonTask41.ForeColor = Color.MidnightBlue;
        }

        public void FortyOneTask_Click(object sender, EventArgs e)
        {   // запуск формы с 41 этюдом
            this.buttonTask41.BackColor = Color.MidnightBlue;
            this.buttonTask41.ForeColor = Color.Yellow;
            Task41 newTask41 = new Task41();
            newTask41.ShowDialog();
        }

        // 42 этюд (ход черных) 
        private void FortyTwoTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask42.BackColor = Color.MidnightBlue;
            this.buttonTask42.ForeColor = Color.Yellow;
        }

        private void FortyTwoTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask42.BackColor = Color.Yellow;
            this.buttonTask42.ForeColor = Color.MidnightBlue;
        }

        public void FortyTwoTask_Click(object sender, EventArgs e)
        {   // запуск формы с 42 этюдом
            this.buttonTask42.BackColor = Color.MidnightBlue;
            this.buttonTask42.ForeColor = Color.Yellow;
            Task42 newTask42 = new Task42();
            newTask42.ShowDialog();
        }

        // 43 этюд (ход черных) 
        private void FortyThreeTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask43.BackColor = Color.MidnightBlue;
            this.buttonTask43.ForeColor = Color.Yellow;
        }

        private void FortyThreeTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask43.BackColor = Color.Yellow;
            this.buttonTask43.ForeColor = Color.MidnightBlue;
        }

        public void FortyThreeTask_Click(object sender, EventArgs e)
        {   // запуск формы с 43 этюдом
            this.buttonTask43.BackColor = Color.MidnightBlue;
            this.buttonTask43.ForeColor = Color.Yellow;
            Task43 newTask43 = new Task43();
            newTask43.ShowDialog();
        }

        // 44 этюд (ход черных) 
        private void FortyFourTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask44.BackColor = Color.MidnightBlue;
            this.buttonTask44.ForeColor = Color.Yellow;
        }

        private void FortyFourTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask44.BackColor = Color.Yellow;
            this.buttonTask44.ForeColor = Color.MidnightBlue;
        }

        public void FortyFourTask_Click(object sender, EventArgs e)
        {   // запуск формы с 44 этюдом
            this.buttonTask44.BackColor = Color.MidnightBlue;
            this.buttonTask44.ForeColor = Color.Yellow;
            Task44 newTask44 = new Task44();
            newTask44.ShowDialog();
        }

        // 45 этюд (ход черных) 
        private void FortyFiveTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask45.BackColor = Color.MidnightBlue;
            this.buttonTask45.ForeColor = Color.Yellow;
        }

        private void FortyFiveTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask45.BackColor = Color.Yellow;
            this.buttonTask45.ForeColor = Color.MidnightBlue;
        }

        public void FortyFiveTask_Click(object sender, EventArgs e)
        {   // запуск формы с 45 этюдом
            this.buttonTask45.BackColor = Color.MidnightBlue;
            this.buttonTask45.ForeColor = Color.Yellow;
            Task45 newTask45 = new Task45();
            newTask45.ShowDialog();
        }

        // 46 этюд (ход черных) 
        private void FortySixTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask46.BackColor = Color.MidnightBlue;
            this.buttonTask46.ForeColor = Color.Yellow;
        }

        private void FortySixTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask46.BackColor = Color.Yellow;
            this.buttonTask46.ForeColor = Color.MidnightBlue;
        }

        public void FortySixTask_Click(object sender, EventArgs e)
        {   // запуск формы с 46 этюдом
            this.buttonTask46.BackColor = Color.MidnightBlue;
            this.buttonTask46.ForeColor = Color.Yellow;
            Task46 newTask46 = new Task46();
            newTask46.ShowDialog();
        }

        // 47 этюд (ход белых)
        private void FourtySevenTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask47.BackColor = Color.MidnightBlue;
            this.buttonTask47.ForeColor = Color.Yellow;
        }

        private void FourtySevenTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask47.BackColor = Color.Yellow;
            this.buttonTask47.ForeColor = Color.MidnightBlue;
        }

        public void FourtySevenTask_Click(object sender, EventArgs e)
        {   // запуск формы с 47 этюдом
            this.buttonTask47.BackColor = Color.MidnightBlue;
            this.buttonTask47.ForeColor = Color.Yellow;
            Task47 newTask47 = new Task47();
            newTask47.ShowDialog();
        }

        // 48 этюд (ход черных) 
        private void FortyEightTask_Leave(object sender, EventArgs e)
        {   // когда курсор покидает кнопку
            this.buttonTask48.BackColor = Color.MidnightBlue;
            this.buttonTask48.ForeColor = Color.Yellow;
        }

        private void FortyEightTask_Hover(object sender, EventArgs e)
        {   // когда курсор над кнопкой
            this.buttonTask48.BackColor = Color.Yellow;
            this.buttonTask48.ForeColor = Color.MidnightBlue;
        }

        public void FortyEightTask_Click(object sender, EventArgs e)
        {   // запуск формы с 48 этюдом
            this.buttonTask48.BackColor = Color.MidnightBlue;
            this.buttonTask48.ForeColor = Color.Yellow;
            Task48 newTask48 = new Task48();
            newTask48.ShowDialog();
        }
    }
}
