using System;
using System.Collections.Generic;
using System.Linq;
using GofL.Properties;

namespace GofL
{
    /// <summary>
    /// Class to represent our grid backend
    /// </summary>
    public class GridBackend
    {
        private readonly Cell[,] _cells;
        private int Rows { get; }
        private int Cols { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The column number</param>
        public GridBackend(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            _cells = new Cell[rows, cols];
        }

        /// <summary>
        /// Get all the cell statuses
        /// In a flatten array
        /// </summary>
        /// <returns>A collection containing all the cell statuses</returns>
        public IEnumerable<Status> GetCellsStatuses()
        {
            return from Cell cell in _cells select cell.Status;
        }

        /// <summary>
        /// Seed our grid following a pattern
        /// TODO handle more patterns
        /// </summary>
        /// <param name="seedPattern">The pattern for seeding</param>
        /// <param name="numberOfRandomCells">The number of cells to put if the seed pattern is set to random</param>
        /// <exception cref="ArgumentOutOfRangeException">If the pattern is not existing</exception>
        public void Seed(SeedPattern seedPattern,int numberOfRandomCells = 10)
        {
            switch (seedPattern)
            {
                case SeedPattern.Random:
                    var rd = new Random();
                    for (var i = 0; i < numberOfRandomCells; i++)
                    {
                        var (x, y) = (rd.Next(Rows - 1), rd.Next(Cols - 1));
                        _cells[x, y] = new Cell()
                        {
                            Status = Status.Living
                        };
                    }

                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(seedPattern), seedPattern, Resources.GridBackend_Seed_Invalid_pattern);
            }
        }

        /// <summary>
        /// Fill all our cells following a pattern
        /// </summary>
        /// <param name="status"></param>
        public void Fill(Status status)
        {
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    _cells[i, j] = new Cell()
                    {
                        Status = status
                    };
                }
            }
        }



        /// <summary>
        /// Make a simulation for all the cells
        /// </summary>
        public void Live()
        {
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    _cells[i, j].Status = GetNextCellStatus(i,j);
                }
            }
        }

        /// <summary>
        /// Compute the next status for a cell depending on siblings
        /// Ignore the exception on case of IndexOutOfRangeException
        /// </summary>
        /// <param name="x">The x axis position</param>
        /// <param name="y">The y axis position</param>
        /// <returns>The status the cell will have</returns>
        private Status GetNextCellStatus(int x, int y)
        {
            var totalLiving = 0;
            for (var i = -1; i < 2; i++)
            {
                for (var j = -1; j < 2; j++)
                {
                    try
                    {
                        totalLiving += _cells[x - i, y - j].Status == Status.Living ? 1 : 0;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }

            return totalLiving == 3 || (_cells[x, y].Status == Status.Living && totalLiving == 2)
                ? Status.Living
                : Status.Dead;
        }
    }
}