using System;
using System.Collections.Generic;
using System.Linq;
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

		public BufferElement(int left, int top, string word)
		{
			Left = left;
			Top = top;
			Word = word;
		}

	}

	public static class ConsoleBuffer
	{

		static List<BufferElement> buffer = new List<BufferElement>();

		static Thread bufferThread = new Thread(ReadFromBuffer);

		static bool running = true;

		#region Management

		static void AddToBuffer(int left, int top, string word)
		{
			try
			{
				buffer.Add(new BufferElement(left, top, word));
			}
			catch { }

		}

		static string ReadKeys(int left, int top)
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
					AddToBuffer(left, top, line+" ");


				}
				else
				{

					line += keyPressed;
					AddToBuffer(left, top, line);

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

		public static string ReadLine(int left, int top)
		{

			return ReadKeys(left, top);

		}

		public static char ReadKey(int left, int top)
		{

			while (Console.KeyAvailable)
			{

				Console.ReadKey(true);

			}

			char keyPressed = Console.ReadKey(true).KeyChar;

			AddToBuffer(left, top, keyPressed.ToString());

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

		public static void Write(int left, int top, string word)
		{

			AddToBuffer(left, top, word);

		}

		#endregion

	}

}
