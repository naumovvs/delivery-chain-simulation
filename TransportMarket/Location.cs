using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportMarket
{
    /// <summary>
    /// Расположение субъекта рынка (класс)
    /// </summary>
    public class Location
    {
        // пример кодирования зон для сетки размером 3*3 (areaSize = 3) 
        // 1  2  3
        // 4  5  6
        // 7  8  9
        
        /// <summary>
        /// координаты зоны
        /// </summary>
        private int x, y;
        /// <summary>
        /// Размер координатной сетки по оси абсцисс
        /// </summary>
        private int areaSize;
                
        /// <summary>
        /// Расположение субъекта рынка
        /// </summary>
        /// <param name="x">координата x</param>
        /// <param name="y">координата y</param>
        /// <param name="ars">размер координатной сетки</param>
        public Location(int x, int y, int ars = 10)
        {
            this.areaSize = ars;
            // координата не может быть больше размера координатной сетки
            if (x > areaSize) this.x = ars; else this.x = x;
            if (y > areaSize) this.y = ars; else this.y = y;
        }

        /// <summary>
        /// Возвращает код зоны месторасположения субъекта рынка
        /// </summary>
        public int Region { get { return x + (y - 1) * areaSize; } }

        /// <summary>
        /// Возвращает размер квадратной координатной сетки,
        /// использующейся для кодировки местоположения субъекта рынка транспортных услуг
        /// </summary>
        public int AreaSize
        { get { return areaSize; } }

    }

}
