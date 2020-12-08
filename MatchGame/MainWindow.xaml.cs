using System;
using System.Collections.Generic;
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

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>.
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetupGame();
        }

        /// <summary>
        /// Updates the elapsed time and stops the timer once the player has found all of the matches.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }
            /// <summary>
            /// SetupGame. The 8 sets of Emoji's are defined in here. Also functionality to generate random layout.
            /// </summary>
            private void SetupGame()
            {
                List<string> animalEmoji = new List<string>()
                {
                    "😺","😺",
                    "🐶","🐶",
                    "🐄","🐄",
                    "👹","👹",
                    "‍🐘","‍🐘",
                    "🙉","🙉",
                    "🐔","🐔",
                    "🐳","🐳",
                };
                Random random = new Random();

                foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
                {
                    if (textBlock.Name != "timeTextBlock")
                    {

                        int index = random.Next(animalEmoji.Count);
                        string nextEmoji = animalEmoji[index];
                        textBlock.Text = nextEmoji;
                        animalEmoji.RemoveAt(index);
                    }
                }

                timer.Start();
                tenthsOfSecondsElapsed = 0;
                matchesFound = 0;
            }

            TextBlock lastTextBlockClicked;
            bool findingMatch = false;
            /// <summary>
            ///  If it's the first in the pair being clicked, keep track of which TextBlock was clicked and make the animal disappear. If it's the second one, either make it disappear (if it's a match) or bring back the first one (if it's not).
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
            {
                TextBlock textBlock = sender as TextBlock;
                if (findingMatch == false)
                {
                    textBlock.Visibility = Visibility.Hidden;
                    lastTextBlockClicked = textBlock;
                    findingMatch = true;
                }
                else if (textBlock.Text == lastTextBlockClicked.Text)
                {
                    matchesFound++;
                    textBlock.Visibility = Visibility.Hidden;
                    findingMatch = false;
                }
                else
                {
                    lastTextBlockClicked.Visibility = Visibility.Visible;
                    findingMatch = false;
                }
            }

            /// <summary>
            /// This resets the game if all 8 matched pairs have been found(otherwise it does nothing because the game is still running).
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
            {
                if (matchesFound == 8)
                {
                    SetupGame();
                }
            }
        }
    }

