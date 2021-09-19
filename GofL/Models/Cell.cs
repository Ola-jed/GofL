namespace GofL
{
    /// <summary>
    /// Cells of our cellular automaton
    /// </summary>
    public struct Cell
    {
        public Status Status
        {
            get => _status;
            set
            {
                _isBorn = value == Status.Living && _status == Status.Dead;
                _status = _isBorn && value == Status.Living ? Status.Emerging : value;
            }
        }

        private Status _status;
        private bool _isBorn;
    }
}