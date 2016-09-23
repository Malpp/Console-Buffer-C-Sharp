using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleBufferNP
{
	public class BufferElement
	{

		public int Left { get; private set; }
		public int Top { get; private set; }
		public string Word { get; private set; }
		public ConsoleColor Color { get; private set; }

		public BufferElement(int left, int top, string word, ConsoleColor color = ConsoleColor.Gray)
		{
			Left = left;
			Top = top;
			Word = word;
			Color = color;
		}

	}

	public static class ConsoleBuffer
	{

		static List<BufferElement> buffer = new List<BufferElement>();

		static Thread bufferThread = new Thread(ReadFromBuffer);

		static bool running = true;

		#region Management

		static void AddToBuffer(int left, int top, string word, ConsoleColor color)
		{
			try
			{
				buffer.Add(new BufferElement(left, top, word, color));
			}
			catch { }

		}

		static string ReadKeys(int left, int top, ConsoleColor color = ConsoleColor.Gray)
		{

            while (Console.KeyAvailable)
            {

                Console.ReadKey(true);

            }

			string line = "";

			while (true)
			{

				char keyPressed = Console.ReadKey(true).KeyChar;

				if (keyPressed == 13)
				{

					break;

				} else if(keyPressed == 8 && line.Length > 0)
                {

					line = line.Remove(line.Length - 1);
					AddToBuffer(left, top, line+" ", color);


				}
				else
				{

					line += keyPressed;
					AddToBuffer(left, top, line, color);

				}

			}

			return line;

		}


		static void ReadFromBuffer()
		{

			while (running)
			{

				if (buffer.Count() != 0)
				{

					try
					{

						Console.SetCursorPosition(buffer[0].Left, buffer[0].Top);
						Console.ForegroundColor = buffer[0].Color;
						Console.Write(buffer[0].Word);

						buffer.RemoveAt(0);

					}
					catch
					{

						try
						{
							buffer.RemoveAt(0);
						}
						catch { }

					}

				}

			}

		}

		#endregion

		#region public functions

		public static string ReadLine(int left, int top, ConsoleColor color = ConsoleColor.Gray)
		{

			return ReadKeys(left, top, color);

		}

		public static char ReadKey(int left, int top, ConsoleColor color = ConsoleColor.Gray)
		{

			while (Console.KeyAvailable)
			{

				Console.ReadKey(true);

			}

			char keyPressed = Console.ReadKey(true).KeyChar;

			AddToBuffer(left, top, keyPressed.ToString(), color);

			return keyPressed;

		}

		public static char ReadKey()
		{

			while (Console.KeyAvailable)
			{

				Console.ReadKey(true);

			}

			return Console.ReadKey(true).KeyChar;

		}

		public static void StartBuffer()
		{

			Console.CursorVisible = false;

			bufferThread.Start();

		}

		public static void StopBuffer()
		{

			running = false;

			bufferThread.Join();

		}

		public static void Write(int left, int top, string word, ConsoleColor color = ConsoleColor.Gray)
		{

			AddToBuffer(left, top, word, color);


		}

		#endregion

	}

}
