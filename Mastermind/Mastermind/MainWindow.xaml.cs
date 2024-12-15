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
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Security.Cryptography.X509Certificates;

namespace Mastermind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string colour1;
        string colour2;
        string colour3;
        string colour4;
        string currentPlayerName = "";
        int attemptsCounter = 10;
        int attemptCounter = 1;
        int secondsCounter = 0;
        int playerCounter = 0;
        int score = 100;
        bool quitBool = false;
        StringBuilder sb = new StringBuilder();
        string[,] highscores = new string[15, 3];
        private DispatcherTimer timer = new DispatcherTimer();
        List<Label> label1List = new List<Label>();
        List<Label> label2List = new List<Label>();
        List<Label> label3List = new List<Label>();
        List<Label> label4List = new List<Label>();
        List<string> playerNamesList = new List<string>();
        
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += Countdown;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        //METHODS
        private void GenerateColours(out string colourSlot1, out string colourSlot2, out string colourSlot3, out string colourSlot4)
        {
            Random rng = new Random();
            List<string> colourList = new List<string>();
            int rngNumber;
            string rngString = "";
            
            for (int i=0; i<4; i++)
            {
                rngNumber = rng.Next(1, 7);
                switch (rngNumber)
                {
                    case 1:
                        rngString = "Red";
                        break;
                    case 2:
                        rngString = "Yellow";
                        break;
                    case 3:
                        rngString = "Orange";
                        break;
                    case 4:
                        rngString = "White";
                        break;
                    case 5:
                        rngString = "Green";
                        break;
                    case 6:
                        rngString = "Blue";
                        break;
                }
                colourList.Add(rngString);
            }
            colourSlot1 = colourList[0];
            colourSlot2 = colourList[1];
            colourSlot3 = colourList[2];
            colourSlot4 = colourList[3];
        }
        private void GenerateBackgrounds(Label label1, Label label2, Label label3, Label label4, out List<string> stringList)
        {
            List<string> colourList = new List<string>();
            colourList.Add(label1.Background.ToString());
            colourList.Add(label2.Background.ToString());
            colourList.Add(label3.Background.ToString());
            colourList.Add(label4.Background.ToString());
            stringList = new List<string>();
            for(int i=0; i<4; i++)
            {
                switch (colourList[i])
                {
                    case "#FFFF0000":
                        stringList.Add("Red");
                        break;
                    case "#FFFFFF00":
                        stringList.Add("Yellow");
                        break;
                    case "#FFFFA500":
                        stringList.Add("Orange");
                        break;
                    case "#FFFFFFFF":
                        stringList.Add("White");
                        break;
                    case "#FF008000":
                        stringList.Add("Green");
                        break;
                    case "#FF0000FF":
                        stringList.Add("Blue");
                        break;
                    default:
                        stringList.Add("Invalid");
                        break;
                }
            }
        }
        
        private void Countdown(object sender, EventArgs e)
        {
            
            secondsCounter++;
            if (secondsCounter % 10 == 0)
            {
                secondsCounter = 0;
                label1List[attemptCounter-1].Background = Brushes.Black;
                label2List[attemptCounter-1].Background = Brushes.Black;
                label3List[attemptCounter - 1].Background = Brushes.Black;
                label4List[attemptCounter - 1].Background = Brushes.Black;
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
                comboBox4.SelectedIndex = -1;
                attemptCounter++;
                attemptLabel.Content = $"Attempt: {attemptCounter}";
                score -= 8;
                scoreLabel.Content = $"Score: {score}";
                if (attemptCounter > attemptsCounter)
                {
                    playerCounter++;
                    if (playerCounter >= playerNamesList.Count)
                    {
                        timer.Stop();
                        MessageBoxResult result = MessageBox.Show($"You have failed! The correct code was: {colour1} {colour2} {colour3} {colour4}. \nYou were the last player in the list.", "FAILED", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        highscores[playerCounter - 1, 0] = playerNamesList[playerCounter - 1];
                        highscores[playerCounter - 1, 1] = (attemptCounter - 1).ToString();
                        highscores[playerCounter - 1, 2] = "0";
                        sb.Append(highscores[playerCounter - 1, 0] + " - " + highscores[playerCounter - 1, 1] + " attempts - " + highscores[playerCounter - 1, 2] + "/100\n");
                        comboBox1.IsEnabled = false;
                        comboBox2.IsEnabled = false;
                        comboBox3.IsEnabled = false;
                        comboBox4.IsEnabled = false;
                        checkButton.IsEnabled = false;
                        hintBigButton.IsEnabled = false;
                        hintSmallButton.IsEnabled = false;
                        ClearLabels();
                        attemptCounter = 1;
                        attemptLabel.Content = "Attempt: 1";
                        secondsCounter = 0;
                        timeLabel.Content = "Seconds: 0";
                        score = 100;
                        scoreLabel.Content = "Score: 100";
                        comboBox1.SelectedIndex = -1;
                        comboBox2.SelectedIndex = -1;
                        comboBox3.SelectedIndex = -1;
                        comboBox4.SelectedIndex = -1;
                        playerNamesList.Clear();
                        playerCounter = 0;
                        nameLabel.Content = "Name: ";
                        return;
                    }
                    else
                    {
                        timer.Stop();
                        MessageBoxResult result = MessageBox.Show($"You have failed! The correct code was: {colour1} {colour2} {colour3} {colour4}. \nNow it's {playerNamesList[playerCounter]}'s turn.", $"{playerNamesList[playerCounter-1]}", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        highscores[playerCounter - 1, 0] = playerNamesList[playerCounter-1];
                        highscores[playerCounter - 1, 1] = (attemptCounter - 1).ToString();
                        highscores[playerCounter - 1, 2] = "0";
                        sb.Append(highscores[playerCounter - 1, 0] + " - " + highscores[playerCounter - 1, 1] + " attempts - " + highscores[playerCounter - 1, 2] + "/100\n");
                        ClearLabels();
                        StartGame();
                        return;
                    }
                    
                }
            }
            timeLabel.Content = $"Seconds: {secondsCounter}";
        }
        private void GenerateLabels(int rowCounter)
        {
            UniformGrid labelGrid = new UniformGrid
            {
                Rows = rowCounter,
                Columns = 4,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };
            for (int i = 0; i < rowCounter; i++)
            {
                for(int j=0; j < 4; j++)
                {
                    Label usableLabel = new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Center,
                        BorderThickness = new Thickness(5),
                        Background = Brushes.Transparent,
                        Width = 115
                    };
                    labelGrid.Children.Add(usableLabel);
                    switch (j)
                    {
                        case 0:
                            label1List.Add(usableLabel);
                            break;
                        case 1:
                            label2List.Add(usableLabel);
                            break;
                        case 2:
                            label3List.Add(usableLabel);
                            break;
                        case 3:
                            label4List.Add(usableLabel);
                            break;
                    }
                }
            }
            labelsStackPannel.Children.Clear();
            labelsStackPannel.Children.Add(labelGrid);
        }
        private void ClearLabels()
        {
            labelsStackPannel.Children.Clear();
           // for (int i=0; i<rowCounter; i++)
            //{
               // labelList[i].Background = Brushes.Transparent;
                //labelList[i].BorderBrush = Brushes.Transparent;
            //}
            label1List.Clear();
            label2List.Clear();
            label3List.Clear();
            label4List.Clear();
        }
        private void StartGame()
        {
            comboBox1.IsEnabled = true;
            comboBox2.IsEnabled = true;
            comboBox3.IsEnabled = true;
            comboBox4.IsEnabled = true;
            checkButton.IsEnabled = true;
            hintBigButton.IsEnabled = true;
            hintSmallButton.IsEnabled = true;
            timer.Start();
            GenerateColours(out colour1, out colour2, out colour3, out colour4);
            GenerateLabels(attemptsCounter);
            solutionTextBox.Text = $"{colour1}, {colour2}, {colour3}, {colour4}";
            attemptCounter = 1;
            attemptLabel.Content = "Attempt: 1";
            secondsCounter = 0;
            timeLabel.Content = "Seconds: 0";
            score = 100;
            scoreLabel.Content = "Score: 100";
            nameLabel.Content = $"Name: {playerNamesList[playerCounter]}";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
        }
        //EVENT METHODS
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int indexAttempt = attemptCounter - 1;
            if (comboBox1.SelectedIndex != -1)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        label1List[indexAttempt].Background = Brushes.Red;
                        break;
                    case 1:
                        label1List[indexAttempt].Background = Brushes.Yellow;
                        break;
                    case 2:
                        label1List[indexAttempt].Background = Brushes.Orange;
                        break;
                    case 3:
                        label1List[indexAttempt].Background = Brushes.White;
                        break;
                    case 4:
                        label1List[indexAttempt].Background = Brushes.Green;
                        break;
                    case 5:
                        label1List[indexAttempt].Background = Brushes.Blue;
                        break;
                }
            }
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int indexAttempt = attemptCounter - 1;
            if (comboBox2.SelectedIndex != -1)
            {
                switch (comboBox2.SelectedIndex)
                {
                    case 0:
                        label2List[indexAttempt].Background = Brushes.Red;
                        break;
                    case 1:
                        label2List[indexAttempt].Background = Brushes.Yellow;
                        break;
                    case 2:
                        label2List[indexAttempt].Background = Brushes.Orange;
                        break;
                    case 3:
                        label2List[indexAttempt].Background = Brushes.White;
                        break;
                    case 4:
                        label2List[indexAttempt].Background = Brushes.Green;
                        break;
                    case 5:
                        label2List[indexAttempt].Background = Brushes.Blue;
                        break;
                }
            }
        }

        private void comboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int indexAttempt = attemptCounter - 1;
            if (comboBox3.SelectedIndex != -1)
            {
                switch (comboBox3.SelectedIndex)
                {
                    case 0:
                        label3List[indexAttempt].Background = Brushes.Red;
                        break;
                    case 1:
                        label3List[indexAttempt].Background = Brushes.Yellow;
                        break;
                    case 2:
                        label3List[indexAttempt].Background = Brushes.Orange;
                        break;
                    case 3:
                        label3List[indexAttempt].Background = Brushes.White;
                        break;
                    case 4:
                        label3List[indexAttempt].Background = Brushes.Green;
                        break;
                    case 5:
                        label3List[indexAttempt].Background = Brushes.Blue;
                        break;
                }
            }
        }

        private void comboBox4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int indexAttempt = attemptCounter - 1;
            if (comboBox4.SelectedIndex != -1)
            {
                switch (comboBox4.SelectedIndex)
                {
                    case 0:
                        label4List[indexAttempt].Background = Brushes.Red;
                        break;
                    case 1:
                        label4List[indexAttempt].Background = Brushes.Yellow;
                        break;
                    case 2:
                        label4List[indexAttempt].Background = Brushes.Orange;
                        break;
                    case 3:
                        label4List[indexAttempt].Background = Brushes.White;
                        break;
                    case 4:
                        label4List[indexAttempt].Background = Brushes.Green;
                        break;
                    case 5:
                        label4List[indexAttempt].Background = Brushes.Blue;
                        break;
                }
            }
        }

        private void checkButton_Click(object sender, RoutedEventArgs e)
        {

            int indexAttempt = attemptCounter - 1;
            attemptCounter += 1;
            attemptLabel.Content = $"Attempt: {attemptCounter}";
            secondsCounter = 0;
            List<string> colourList = new List<string>();
            colourList.Add(colour1);
            colourList.Add(colour2);
            colourList.Add(colour3);
            colourList.Add(colour4);
            List<string> backgroundList = new List<string>();
            GenerateBackgrounds(label1List[indexAttempt], label2List[indexAttempt], label3List[indexAttempt], label4List[indexAttempt], out backgroundList);
            if (backgroundList.Contains("Invalid"))
            {
                label1List[indexAttempt].Background = Brushes.Black;
                label2List[indexAttempt].Background = Brushes.Black;
                label3List[indexAttempt].Background = Brushes.Black;
                label4List[indexAttempt].Background = Brushes.Black;
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
                comboBox4.SelectedIndex = -1;
                score -= 8;
                scoreLabel.Content = $"Score: {score}";
                if (attemptCounter > attemptsCounter)
                {
                    playerCounter++;
                    if (playerCounter >= playerNamesList.Count)
                    {
                        timer.Stop();
                        MessageBoxResult result = MessageBox.Show($"You have failed! The correct code was: {colour1} {colour2} {colour3} {colour4}. \nYou were the last player in the list.", "FAILED", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        highscores[playerCounter - 1, 0] = playerNamesList[playerCounter - 1];
                        highscores[playerCounter - 1, 1] = (attemptCounter - 1).ToString();
                        highscores[playerCounter - 1, 2] = "0";
                        sb.Append(highscores[playerCounter - 1, 0] + " - " + highscores[playerCounter - 1, 1] + " attempts - " + highscores[playerCounter - 1, 2] + "/100\n");
                        comboBox1.IsEnabled = false;
                        comboBox2.IsEnabled = false;
                        comboBox3.IsEnabled = false;
                        comboBox4.IsEnabled = false;
                        checkButton.IsEnabled = false;
                        hintBigButton.IsEnabled = false;
                        hintSmallButton.IsEnabled = false;
                        ClearLabels();
                        playerNamesList.Clear();
                        attemptCounter = 1;
                        attemptLabel.Content = "Attempt: 1";
                        secondsCounter = 0;
                        timeLabel.Content = "Seconds: 0";
                        score = 100;
                        scoreLabel.Content = "Score: 100";
                        comboBox1.SelectedIndex = -1;
                        comboBox2.SelectedIndex = -1;
                        comboBox3.SelectedIndex = -1;
                        comboBox4.SelectedIndex = -1;
                        playerCounter = 0;
                        nameLabel.Content = "Name: ";
                        return;
                    }
                    else
                    {
                        timer.Stop();
                        MessageBoxResult result = MessageBox.Show($"You have failed! The correct code was: {colour1} {colour2} {colour3} {colour4}. \nNow it's {playerNamesList[playerCounter]}'s turn.", $"{playerNamesList[playerCounter - 1]}", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        highscores[playerCounter - 1, 0] = playerNamesList[playerCounter - 1];
                        highscores[playerCounter - 1, 1] = (attemptCounter - 1).ToString();
                        highscores[playerCounter - 1, 2] = "0";
                        sb.Append(highscores[playerCounter - 1, 0] + " - " + highscores[playerCounter - 1, 1] + " attempts - " + highscores[playerCounter - 1, 2] + "/100\n");
                        ClearLabels();
                        StartGame();
                        return;
                    }
                }
                MessageBox.Show("At least one of the combo boxes is empty, try again.");
                return;
            }
            List<string> borderList = new List<string>();
            for (int i=0; i<4; i++)
            {
                if (backgroundList[i] == colourList[i])
                {
                    borderList.Add("DarkRed");
                }
                else if (colourList.Contains(backgroundList[i]))
                {
                    borderList.Add("Wheat");
                }
                else borderList.Add("None");
            }
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        if (borderList[i] == "DarkRed")
                        {
                            label1List[indexAttempt].BorderBrush = Brushes.DarkRed;
                        }
                        else if (borderList[i] == "Wheat")
                        {
                            label1List[indexAttempt].BorderBrush = Brushes.Wheat;
                            score -= 1;
                        }
                        else
                        {
                            label1List[indexAttempt].BorderBrush = Brushes.Transparent;
                            score -= 2;
                        }
                        break;
                    case 1:
                        if (borderList[i] == "DarkRed")
                        {
                            label2List[indexAttempt].BorderBrush = Brushes.DarkRed;
                        }
                        else if (borderList[i] == "Wheat")
                        {
                            label2List[indexAttempt].BorderBrush = Brushes.Wheat;
                            score -= 1;
                        }
                        else
                        {
                            label2List[indexAttempt].BorderBrush = Brushes.Transparent;
                            score -= 2;
                        }
                        break;
                    case 2:
                        if (borderList[i] == "DarkRed")
                        {
                            label3List[indexAttempt].BorderBrush = Brushes.DarkRed;
                        }
                        else if (borderList[i] == "Wheat")
                        {
                            label3List[indexAttempt].BorderBrush = Brushes.Wheat;
                            score -= 1;
                        }
                        else
                        {
                            label3List[indexAttempt].BorderBrush = Brushes.Transparent;
                            score -= 2;
                        }
                        break;
                    case 3:
                        if (borderList[i] == "DarkRed")
                        {
                            label4List[indexAttempt].BorderBrush = Brushes.DarkRed;
                        }
                        else if (borderList[i] == "Wheat")
                        {
                            label4List[indexAttempt].BorderBrush = Brushes.Wheat;
                            score -= 1;
                        }
                        else
                        {
                            label4List[indexAttempt].BorderBrush = Brushes.Transparent;
                            score -= 2;
                        }
                        break;
                }
            }
            if (borderList[0] == "DarkRed" && borderList[1] == "DarkRed" && borderList[2] == "DarkRed" && borderList[3] == "DarkRed")
            {
                playerCounter++;
                if (playerCounter >= playerNamesList.Count)
                {
                    timer.Stop();
                    MessageBox.Show($"The correct code has been found in {attemptCounter.ToString()} attempts. \nYou were the last player in the list.", "Winner", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    highscores[playerCounter - 1, 0] = playerNamesList[playerCounter - 1];
                    highscores[playerCounter - 1, 1] = (attemptCounter - 1).ToString();
                    highscores[playerCounter - 1, 2] = score.ToString();
                    sb.Append(highscores[playerCounter - 1, 0] + " - " + highscores[playerCounter - 1, 1] + " attempts - " + highscores[playerCounter - 1, 2] + "/100\n");
                    comboBox1.IsEnabled = false;
                    comboBox2.IsEnabled = false;
                    comboBox3.IsEnabled = false;
                    comboBox4.IsEnabled = false;
                    checkButton.IsEnabled = false;
                    hintBigButton.IsEnabled = false;
                    hintSmallButton.IsEnabled = false;
                    ClearLabels();
                    playerNamesList.Clear();
                    attemptCounter = 1;
                    attemptLabel.Content = "Attempt: 1";
                    secondsCounter = 0;
                    timeLabel.Content = "Seconds: 0";
                    score = 100;
                    scoreLabel.Content = "Score: 100";
                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;
                    comboBox3.SelectedIndex = -1;
                    comboBox4.SelectedIndex = -1;
                    playerCounter = 0;
                    nameLabel.Content = "Name: ";
                    return;
                }
                else
                {
                    timer.Stop();
                    MessageBox.Show($"The correct code has been found in {attemptCounter.ToString()} attempts.. \nNow it's {playerNamesList[playerCounter]}'s turn.", $"{playerNamesList[playerCounter - 1]}", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    highscores[playerCounter - 1, 0] = playerNamesList[playerCounter - 1];
                    highscores[playerCounter - 1, 1] = (attemptCounter - 1).ToString();
                    highscores[playerCounter - 1, 2] = score.ToString();
                    sb.Append(highscores[playerCounter - 1, 0] + " - " + highscores[playerCounter - 1, 1] + " attempts - " + highscores[playerCounter - 1, 2] + "/100\n");
                    ClearLabels();
                    StartGame();
                    return;
                }
            }
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            scoreLabel.Content = $"Score: {score}";
        }
        private void ToggleDebug(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.F12)
                {
                    if (solutionTextBox.Visibility == Visibility.Hidden)
                    {
                        solutionTextBox.Visibility = Visibility.Visible;
                    }
                    else solutionTextBox.Visibility = Visibility.Hidden;
                }
            }
        }

        private void mastermindWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (quitBool == false)
            {
                e.Cancel = true;
                if (MessageBox.Show("Continue closing the window?", "CLOSING WINDOW", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    e.Cancel = false;
                };
            }
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            OpenNameWindow();
            StartGame();
        }

        private void HighscoresClick(object sender, RoutedEventArgs e)
        {
            if (highscores[0,0] != null)
            {
                MessageBox.Show($"{sb.ToString()}", "MASTERMIND HIGHSCORES", MessageBoxButton.OK);
            }
            else MessageBox.Show("There are no highscores set yet, please play the game first!", "HIGHSCORES EMPTY", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeAttemptsClick(object sender, RoutedEventArgs e)
        {
            OpenAttemptsWindow();
        }

        private void OpenAttemptsWindow()
        {
            Window attemptsWindow = new Window
            {
                Title = "Enter the numbers of attempts",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Owner = this
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            TextBlock instructionText = new TextBlock
            {
                Text = "Please enter a number of attempts:",
                Margin = new Thickness(0, 0, 0, 10)
            };
            TextBox inputTextBox = new TextBox { HorizontalAlignment = HorizontalAlignment.Stretch };
            Button okButton = new Button { Content = "OK", Width = 75, Margin = new Thickness(5) };
            Button cancelButton = new Button { Content = "Cancel", Width = 75, Margin = new Thickness(5) };
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);
            panel.Children.Add(instructionText);
            panel.Children.Add(inputTextBox);
            panel.Children.Add(buttonPanel);
            attemptsWindow.Content = panel;
            okButton.Click += (sender, e) =>
            {
                if (int.TryParse(inputTextBox.Text, out int result))
                {
                    if (result > 20 || result < 3)
                    {
                        MessageBox.Show("Please enter a number between 3 and 20.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        attemptsCounter = result;
                        attemptsWindow.DialogResult = true;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            cancelButton.Click += (sender, e) =>
            {
                attemptsWindow.DialogResult = false; 
            };

            bool? dialogResult = attemptsWindow.ShowDialog();

            if (dialogResult == true)
            {
                MessageBox.Show($"New number of attempts: {attemptsCounter}", "Input Stored", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Input canceled.", "No Input", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void OpenNameWindow()
        {
            while (true)
            {
                Window nameDialog = new Window
                {
                    Title = "Enter a Name",
                    Width = 300,
                    Height = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.NoResize,
                    Owner = this
                };

                StackPanel panel = new StackPanel { Margin = new Thickness(10) };

                TextBlock instructionText = new TextBlock
                {
                    Text = "Please enter a name (or click 'No' to stop):",
                    Margin = new Thickness(0, 0, 0, 10)
                };

                TextBox inputTextBox = new TextBox { HorizontalAlignment = HorizontalAlignment.Stretch };

                Button okButton = new Button { Content = "OK", Width = 75, Margin = new Thickness(5) };
                Button noButton = new Button { Content = "No", Width = 75, Margin = new Thickness(5) };

                StackPanel buttonPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                buttonPanel.Children.Add(okButton);
                buttonPanel.Children.Add(noButton);

                panel.Children.Add(instructionText);
                panel.Children.Add(inputTextBox);
                panel.Children.Add(buttonPanel);

                nameDialog.Content = panel;

                bool? dialogResult = null;

                okButton.Click += (sender, e) =>
                {
                    string input = inputTextBox.Text.Trim();

                    if (!string.IsNullOrEmpty(input))
                    {
                        dialogResult = true;
                        nameDialog.DialogResult = true; 
                    }
                    else
                    {
                        MessageBox.Show("Name cannot be empty.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                };
                noButton.Click += (sender, e) =>
                {
                    dialogResult = false;
                    nameDialog.DialogResult = false;
                };

                dialogResult = nameDialog.ShowDialog();

                if (dialogResult == true)
                {
                    string name = inputTextBox.Text.Trim();

                    if (string.Equals(name, "no", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Name input stopped.", "Finished", MessageBoxButton.OK, MessageBoxImage.Information);
                        break; 
                    }

                    playerNamesList.Add(name);
                    MessageBox.Show($"You entered: {name}. It has been added to the list.", "Name Added", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (playerNamesList.Count > 0)
                    {
                        MessageBox.Show("Name input stopped.", "Finished", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Please enter at least 1 player name before continuing.", "Not enough players", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            MessageBox.Show($"Names entered: {string.Join(", ", playerNamesList)}", "Final List", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void hintSmallButton_Click(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            int position = rng.Next(1, 5);
            string colourString = string.Empty;
            switch (position)
            {
                case 1:
                    colourString = colour1;
                    break;
                case 2:
                    colourString = colour2;
                    break;
                case 3:
                    colourString = colour3;
                    break;
                case 4:
                    colourString = colour4;
                    break;
            }
            MessageBox.Show($"The colour {colourString} is present!", "Small Hint!", MessageBoxButton.OK, MessageBoxImage.Information);
            score -= 15;
            scoreLabel.Content = $"Score: {score}";
        }

        private void hintBigButton_Click(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            int position = rng.Next(1, 5);
            string colourString = string.Empty;
            switch (position)
            {
                case 1:
                    colourString = colour1;
                    break;
                case 2:
                    colourString = colour2;
                    break;
                case 3:
                    colourString = colour3;
                    break;
                case 4:
                    colourString = colour4;
                    break;
            }
            MessageBox.Show($"The colour {colourString} is present on position {position}!", "Big Hint!", MessageBoxButton.OK, MessageBoxImage.Information);
            score -= 25;
            scoreLabel.Content = $"Score: {score}";
        }
    }
}

