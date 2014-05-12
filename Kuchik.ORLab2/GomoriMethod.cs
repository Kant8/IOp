using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kuchik.ORLab2.Interfaces;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Kuchik.ORLab2
{
    public class GomoriMethod
    {
        private const double Eps = 1.0e-07;

        private IGomoriInitializer _task;
        private TextWriter _writer;
        private int iterationNumber = 0;
        private List<ArtJEntry> _artJ;     // Key = j, value = limitation number

        public void Solve(IGomoriInitializer initializer, TextWriter writer)
        {
            iterationNumber = 0;
            _task = initializer.Clone();
            _writer = writer;
            _artJ = new List<ArtJEntry>();
            while (GomoriIteration())   // Производим итерации в цикле, пока 
            {                           // не будет найдено решение

            }
        }

        private bool GomoriIteration()
        {
            // Шаг 1
            _writer.WriteLine("Iteration: {0}", iterationNumber);
            var simplexMethod = new SimplexMethod(_writer);
            simplexMethod.Solve(_task);     // Решаем задачу симплекс методом

            _writer.WriteLine("Optimal plan is found: {0}", _task.xo);
            _writer.WriteLine("Target function value = {0}", _task.c * _task.xo);

            // Шаг 2
            //var artJToRemoveRow = -1;
            //var artJToRemoveColumn = -1;
            //artJToRemoveRow = -1;
            //artJToRemoveColumn = -1;

            //foreach (var artJ in _artJ)
            //{
            //    if (_task.Jb.Contains(artJ.Column))
            //    {
            //        var rowToRemove = artJ.Row;     // TODO probably need to rewrite row selection

            //        var ai = _task.A.Row(rowToRemove); // Выбираем строку с искусственым ограничением
            //        ai = -ai / ai[artJ.Column];
            //        var rowList = ai.ToList();
            //        rowList.RemoveAt(artJ.Column);
            //        ai = DenseVector.OfEnumerable(rowList);

            //        var aj = _task.A.Column(artJ.Column);   // Выбираем столбец с искусственным ограничением
            //        var columnList = aj.ToList();
            //        var bCoef = _task.b[rowToRemove] / columnList[rowToRemove];   
            //        columnList.RemoveAt(rowToRemove);  
            //        aj = DenseVector.OfEnumerable(columnList);

            //        var newA = DenseMatrix.Create(_task.A.RowCount - 1, _task.A.ColumnCount - 1,
            //            (i, j) => _task.A[i < rowToRemove ? i : i + 1, j < artJ.Column ? j : j + 1]); 

            //        newA += DenseMatrix.OfMatrix(aj.ToColumnMatrix() * ai.ToRowMatrix());   // Удаляем искусственные строку
            //        _task.A = newA;                                                         // и столбец из матрицы А
            //        _task.b = DenseVector.Create(_task.b.Count - 1, i => i < rowToRemove ? _task.b[i] : _task.b[i + 1]);   
            //        _task.b += bCoef * aj;

            //        _task.c = DenseVector.Create(_task.c.Count - 1, i => i < artJ.Column ? _task.c[i] : _task.c[i + 1]);    // Удаляем искусственную переменную из вектора с

            //        _task.xo = DenseVector.Create(_task.xo.Count - 1, i => i < artJ.Column ? _task.xo[i] : _task.xo[i + 1]);    // Удаляем искусственную переменную из xo

            //        _task.Jb.Remove(artJ.Column);
            //        artJToRemoveColumn = artJ.Column;
            //        artJToRemoveRow = artJ.Row;
            //        break;
            //    }
            //}

            //if (artJToRemoveRow > 0)        // Удаляем искусственную переменную из базисных
            //{
            //    _artJ.RemoveAll(x => x.Row == artJToRemoveRow);
            //    for (int i = 0; i < _artJ.Count; i++)
            //    {
            //        if (_artJ[i].Row > artJToRemoveRow)
            //        {
            //            _artJ[i].Row--;         // Сдвигаем индексы базисных переменных на один 
            //            _artJ[i].Column--;      // После удаления искусственной переменной
            //        }
            //    }

            //    for (int i = 0; i < _task.Jb.Count; i++)
            //    {
            //        _task.Jb[i] = _task.Jb[i] > artJToRemoveColumn ? _task.Jb[i] - 1 : _task.Jb[i];
            //    }
            //}

            // Шаг 3
            var falseIndex = -1;
            var maxFract = 0d;
            for (int i = 0; i < _task.xo.Count(); i++)
            {
                if (Math.Abs(Math.Round(_task.xo[i]) - _task.xo[i]) > Eps)
                {
                    var fract = Math.Abs(_task.xo[i] - Math.Floor(_task.xo[i]));    // Находим базисную переменную
                    if (_task.Jb.Contains(i) && fract > Eps)                        // С максимальной дробной частью
                    {                                                               // и запоминаем ее индекс
                        if (fract > maxFract)
                        {
                            maxFract = fract;
                            falseIndex = i;
                        }
                    }
                }
            }

            if (falseIndex < 0)     // Если все переменные целые - решение найдено
            {
                return false;   // Прерываем выполнение метода
            }
            _writer.WriteLine("Jk = {0}", falseIndex);

            // Шаг 4
            var aB = new DenseMatrix(_task.Jb.Count());
            int index = 0;
            foreach (var j in _task.Jb)
            {
                aB.SetColumn(index, _task.A.Column(j));     // Формируем матрицу Ab из базисных столбцов А
                index++;
            }
            _writer.Write("Jb: ");
            _task.Jb.ForEach(x => _writer.Write("{0} ", x));
            _writer.WriteLine();
            _writer.WriteLine("Basis matrix: {0}", aB);
            var y = DenseMatrix.Identity(_task.A.RowCount).Column(_task.Jb.IndexOf(falseIndex)) * aB.Inverse(); //Находим e'*Ab

            var newRow = new DenseVector(_task.A.ColumnCount + 1);
            newRow.SetSubVector(0, _task.A.ColumnCount, y * _task.A);   // Находим данные для нового отсекающего ограничения

            _writer.WriteLine("Data for new limitation: {0}", newRow);

            for (int i = 0; i < newRow.Count; i++)      // Формируем новое отсекающее ограничение
            {
                if (i < _task.A.ColumnCount)
                {
                    if (Math.Abs(newRow[i]) < Eps)
                    {
                        newRow[i] = 0;
                    }
                    else
                    {
                        newRow[i] = newRow[i] > 0
                                    ? -(newRow[i] - Math.Floor(newRow[i]))
                                    : -(Math.Ceiling(Math.Abs(newRow[i])) - Math.Abs(newRow[i]));
                    }
                }
                else
                {
                    newRow[i] = 1;
                }
            }
            newRow[falseIndex] = 0;
            _writer.WriteLine("New limitation: {0}", newRow);

            var newb = (y * _task.b);   // Находим новый элемент вектора b
            newb = newb > 0 ? -(newb - Math.Floor(newb)) : -(Math.Ceiling(Math.Abs(newb)) - Math.Abs(newb)); // TODO probably need to rewrite this

            _writer.WriteLine("New b = {0}", newb);

            // Шаг 5
            var newMatrix = new DenseMatrix(_task.A.RowCount + 1, _task.A.ColumnCount + 1); // Формируем новую
            newMatrix.SetSubMatrix(0, _task.A.RowCount, 0, _task.A.ColumnCount, _task.A);   // матрицу А
            newMatrix.SetRow(_task.A.RowCount, newRow);
            newMatrix[_task.A.RowCount, _task.A.ColumnCount] = 1;

            var newBVector = new DenseVector(_task.b.Count + 1);    // Формируем новый
            newBVector.SetSubVector(0, _task.b.Count, _task.b);     // вектор b
            newBVector[_task.b.Count] = newb;

            var newCVector = new DenseVector(_task.c.Count + 1);    // Добавляем новую
            newCVector.SetSubVector(0, _task.c.Count, _task.c);     // компоненту вектора с

            var newJb = _task.Jb.ToList();
            newJb.Add(newJb[newJb.Count - 1] + 1);
            _artJ.Add(new ArtJEntry { Column = newMatrix.ColumnCount - 1, Row = newMatrix.RowCount - 1 });

            _task.A = newMatrix.Clone();        // Создаем
            _task.b = newBVector.Clone();       // новую задачу
            _task.c = newCVector.Clone();       // для следующей итерации
            _task.Jb = newJb;

            iterationNumber++;              // Присваиваем новый номер итерации

            return true;
        }

        private class ArtJEntry
        {
            public int Column { get; set; }
            public int Row { get; set; }
        }
    }
}
