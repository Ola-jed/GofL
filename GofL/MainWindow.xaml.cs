using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GofL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// For better patterns, the grid should be a square i.e, Columns = Rows
        /// </summary>
        private const int Columns = 15;
        private const int Rows = Columns;
        private readonly GridBackend _gridBackend = new(Rows,Columns);
        private readonly Dictionary<Status, SolidColorBrush> _statusColors = new()
        {
            { Status.Living ,Brushes.Blue},
            { Status.Dead ,Brushes.Red},
            { Status.Emerging ,Brushes.LightGreen}
        };
        private readonly DispatcherTimer _automatonTimer = new();

        public MainWindow()
        {
            InitializeComponent();
            _automatonTimer.Tick += Tick;
            Grid.Columns = Columns;
            Grid.Rows = Rows;
            _gridBackend.Fill(Status.Dead);
            InitCells();
        }

        /// <summary>
        /// Init the cells (redraw them)
        /// </summary>
        private void InitCells()
        {
            Grid.Children.Clear();
            const int size = Columns * Rows;
            for (ushort i = 0; i < size; i++)
            {
                Grid.Children.Add(new Rectangle()
                {
                    Margin = new Thickness(2),
                    Fill = Brushes.DarkRed
                });
            }
        }

        /// <summary>
        /// Start the simulation when the "Apply" button is clicked
        /// Handle the choice made by the user
        /// Cast the combobox to enum and handle
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event args</param>
        private void StartAutomaton(object sender, RoutedEventArgs e)
        {
            var chosenPattern = PatternComboBox.Text;
            var isValid = Enum.TryParse(chosenPattern, out SeedPattern pattern);
            if (!isValid)
            {
                MessageBox.Show("Choose a pattern before");
                return;
            }
            _gridBackend.Seed(pattern);
            UpdateUi(_gridBackend.GetCellsStatuses());
            _automatonTimer.Interval = TimeSpan.FromSeconds(1);
            _automatonTimer.IsEnabled = true;
        }

        /// <summary>
        /// Method called each times the timer ticks
        /// Run the grid simulation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tick(object sender, EventArgs e)
        {
            _gridBackend.Live();
            UpdateUi(_gridBackend.GetCellsStatuses());
            _automatonTimer.Interval = TimeSpan.FromSeconds(1);
            _automatonTimer.IsEnabled = true;
        }

        /// <summary>
        /// Update the rectangles displayed in the ui
        /// </summary>
        /// <param name="cellsStatuses"></param>
        private void UpdateUi(IEnumerable<Status> cellsStatuses)
        {
            cellsStatuses.Zip(Grid.Children.Cast<UIElement>().ToList(),(s,e) => new {Status = s,Cell = e})
                .ToList()
                .ForEach(t => ((Rectangle)t.Cell).Fill = _statusColors[t.Status]);
        }

        /// <summary>
        /// Stop the automaton
        /// </summary>
        /// <param name="sender">The btn clicked</param>
        /// <param name="e">Arguments of the click</param>
        private void Stop(object sender, RoutedEventArgs e)
        {
            _automatonTimer.Stop();
        }
    }
}