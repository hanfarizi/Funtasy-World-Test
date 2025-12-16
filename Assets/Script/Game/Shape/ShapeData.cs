using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class row
    {
        public bool[] columns;
        public int _size;

        public row()
        {

        }

        public row(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            _size = size;
            columns = new bool[_size];
            ClearRow();
        }

        public void ClearRow()
        {
            for (int i = 0; i < _size; i++)
            {
                columns[i] = false;
            }
        }


    }


    public  int columns= 0;
    public  int rows= 0;
    public  row[] board;

    public void Clear()
    {
        for(var i =0;i< rows; i++)
        {
            board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board = new row[rows];

        for (var i = 0; i < rows; i++)
        {
            board[i] = new row(columns);
        }
    }
}
