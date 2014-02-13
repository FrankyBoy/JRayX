using System.Text;
using JRayXLib.Math;

namespace JRayXLib.Common
{
    public class Matrix4
    {

        public override int GetHashCode()
        {
            return (_data != null ? _data.GetHashCode() : 0);
        }

        private readonly double[,] _data;

        public Matrix4(Matrix4 m)
            : this()
        {
            double[,] dm = m.GetData();

            _data[0, 0] = dm[0, 0];
            _data[1, 0] = dm[1, 0];
            _data[2, 0] = dm[2, 0];
            _data[3, 0] = dm[3, 0];
            _data[0, 1] = dm[0, 1];
            _data[1, 1] = dm[1, 1];
            _data[2, 1] = dm[2, 1];
            _data[3, 1] = dm[3, 1];
            _data[0, 2] = dm[0, 2];
            _data[1, 2] = dm[1, 2];
            _data[2, 2] = dm[2, 2];
            _data[3, 2] = dm[3, 2];
            _data[0, 3] = dm[0, 3];
            _data[1, 3] = dm[1, 3];
            _data[2, 3] = dm[2, 3];
            _data[3, 3] = dm[3, 3];
        }

        public Matrix4()
        {
            _data = new double[4, 4];
        }

        /**
     * @return the data
     */
        public double[,] GetData()
        {
            return _data;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("(");

            for (int row = 0; row < _data.GetLength(0); row++)
            {
                for (int col = 0; col < _data.GetLength(1); col++)
                {
                    sb.Append(string.Format("{0:0.##} ", _data[row, col]));
                }
                sb.Append("\n ");
            }
            sb.Append(")");

            return sb.ToString();
        }

        public bool Equals(Matrix4 other)
        {

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (System.Math.Abs(_data[i, j] - other.GetData()[i, j]) > Constants.EPS)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
