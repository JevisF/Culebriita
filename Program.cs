using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Culebriita
{
    class Program
    {



        internal enum Direction
        {
            Abajo, Izquierda, Derecha, Arriba
        }


        private static void DibujaPantalla(Size size)
        {
            Console.Title = "Culebrita By: Jevis Florian";
            Console.WindowHeight = size.Height + 2;
            Console.WindowWidth = size.Width + 2;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Black;
            for (int row = 0; row < size.Height; row++)
            {
                for (int col = 0; col < size.Width; col++)
                {
                    Console.SetCursorPosition(col + 1, row + 1);
                    Console.Write(" ");
                }
            }
        }
        private static void MuestraPunteo(int punteo)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(17, 0);
            Console.Write($"Gusanos Devorados: {punteo.ToString("00000000")}");
        }


        private static Direction ObtieneDireccion(Direction direccionAcutal)
        {
            if (!Console.KeyAvailable) return direccionAcutal;

            var tecla = Console.ReadKey(true).Key;
            switch (tecla)
            {
                //case ConsoleKey.DownArrow:
                //    if (direccionAcutal != Direction.Arriba)
                //        direccionAcutal = Direction.Abajo;
                //    break;
                //case ConsoleKey.LeftArrow:
                //    if (direccionAcutal != Direction.Derecha)
                //        direccionAcutal = Direction.Izquierda;
                //    break;
                //case ConsoleKey.RightArrow:
                //    if (direccionAcutal != Direction.Izquierda)
                //        direccionAcutal = Direction.Derecha;
                //    break;
                //case ConsoleKey.UpArrow:
                //    if (direccionAcutal != Direction.Abajo)
                //        direccionAcutal = Direction.Arriba;
                //    break;

                case ConsoleKey.DownArrow:
                    direccionAcutal = Direction.Abajo;
                    break;

                case ConsoleKey.LeftArrow:
                    direccionAcutal = Direction.Izquierda;
                    break;
                case ConsoleKey.RightArrow:
                    direccionAcutal = Direction.Derecha;
                    break;
                case ConsoleKey.UpArrow:
                    direccionAcutal = Direction.Arriba;
                    break;


            }
            return direccionAcutal;
        }



        private static Point ObtieneSiguienteDireccion(Direction direction, Point currentPosition)
        {
            Point siguienteDireccion = new Point(currentPosition.X, currentPosition.Y);
            switch (direction)
            {
                case Direction.Arriba:
                    siguienteDireccion.Y--;
                    break;
                case Direction.Izquierda:
                    siguienteDireccion.X--;
                    break;
                case Direction.Abajo:
                    siguienteDireccion.Y++;
                    break;
                case Direction.Derecha:
                    siguienteDireccion.X++;
                    break;
            } //listo arrib
            return siguienteDireccion;
        }



        private static bool MoverLaCulebrita(List<Point> culebra, Point posiciónObjetivo,
            int longitudCulebra, Size screenSize)
        {
            var lastPoint = culebra.Last();

            if (lastPoint == posiciónObjetivo) return true;

            if (culebra.Any(x => x.Equals(posiciónObjetivo))) return false;

            if (posiciónObjetivo.X < 0 || posiciónObjetivo.X >= screenSize.Width
                    || posiciónObjetivo.Y < 0 || posiciónObjetivo.Y >= screenSize.Height)
            {
                return false;
            }

            Console.BackgroundColor = ConsoleColor.White;
            Console.SetCursorPosition(lastPoint.X + 1, lastPoint.Y + 1);
            Console.WriteLine(" ");

            culebra.Add(posiciónObjetivo);

            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(posiciónObjetivo.X + 1, posiciónObjetivo.Y + 1);
            Console.Write(" ");

            // Quitar cola
            if (culebra.Count > longitudCulebra)
            {
                //var removePoint = culebra.Dequeue();
                var removePoint = culebra[0];   //Se desencola
                culebra.RemoveAt(0);


                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(removePoint.X + 1, removePoint.Y + 1);
                Console.Write(" ");
            }
            return true;
        }


        private static Point MostrarComida(Size screenSize, List<Point> culebra)
        {
            var lugarComida = Point.Empty;
            var cabezaCulebra = culebra.Last();
            var rnd = new Random();
            do
            {
                var x = rnd.Next(0, screenSize.Width - 1);
                var y = rnd.Next(0, screenSize.Height - 1);
                if (culebra.All(p => p.X != x || p.Y != y)
                    && Math.Abs(x - cabezaCulebra.X) + Math.Abs(y - cabezaCulebra.Y) > 8)
                {
                    lugarComida = new Point(x, y);
                }

            } while (lugarComida == Point.Empty);

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(lugarComida.X + 1, lugarComida.Y + 1);
            Console.Write(" ");

            return lugarComida;
        }






        static void Main()
        {
            var punteo = 0;
            //var velocidad = 190;
            var velocidad = 125; //modificar estos valores y ver qué pasa
            var posiciónComida = Point.Empty; //
            var tamañoPantalla = new Size(60, 20);
            var culebrita = new List<Point>();
            //var culebrita = new Queue<Point>();

            var longitudCulebra = 10; //modificar estos valores y ver qué pasa
            var posiciónActual = new Point(30, 10); //modificar estos valores y ver qué pasa


            //culebrita.Enqueue(posiciónActual);
            culebrita.Add(posiciónActual);


            //var dirección = Direction.Derecha;*/ //modificar estos valores y ver qué pasa
            var dirección = Direction.Izquierda;      //al modificar podemos elegir el punto de partida de la serpiente arriba abajo, iz, dcha.


            DibujaPantalla(tamañoPantalla);
            MuestraPunteo(punteo);

            while (MoverLaCulebrita(culebrita, posiciónActual, longitudCulebra, tamañoPantalla))
            {
                Thread.Sleep(velocidad);
                dirección = ObtieneDireccion(dirección);
                posiciónActual = ObtieneSiguienteDireccion(dirección, posiciónActual);

                //if (posiciónActual.Equals(posiciónComida))  // Se cambio el metodo de comparacion
                if (posiciónActual == posiciónComida)

                {

                    posiciónComida = Point.Empty;
                    longitudCulebra++;
                    // punteo += 10; //modificar estos valores y ver qué pasa
                    punteo += 20;
                    MuestraPunteo(punteo);
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"beep.wav"); player.Play();
                    //ya que es donde esta el clip, BEEP

                    velocidad -= 15; // mediante esa linea se incrementa la velocidad cada vez que come
                }

                if (posiciónComida == Point.Empty) //entender qué hace esta linea
                                                   //Aparecera un punto de comida en que caso que desaparezca de la pantalla 
                {
                    posiciónComida = MostrarComida(tamañoPantalla, culebrita);
                }
            }

            Console.ResetColor();
            Console.SetCursorPosition(tamañoPantalla.Width / 2 - 4, tamañoPantalla.Height / 2);
            Console.Write("Haz Perdido ");
            Thread.Sleep(2000);
            Console.ReadKey();


        }


    }//end class
}













