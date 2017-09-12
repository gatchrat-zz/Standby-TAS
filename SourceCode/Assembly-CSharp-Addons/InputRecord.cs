using System;
using System.Text;

namespace TAS
{
	// Token: 0x02000004 RID: 4
	public class InputRecord
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000211D File Offset: 0x0000031D
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002125 File Offset: 0x00000325
		public int Line { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000212E File Offset: 0x0000032E
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002136 File Offset: 0x00000336
		public int Frames { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000213F File Offset: 0x0000033F
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002147 File Offset: 0x00000347
		public Actions Actions { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002150 File Offset: 0x00000350
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002158 File Offset: 0x00000358
		public float Angle { get; set; }

		// Token: 0x06000017 RID: 23 RVA: 0x00002161 File Offset: 0x00000361
		public InputRecord()
		{
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026E0 File Offset: 0x000008E0
		public InputRecord(int number, string line)
		{
			this.Line = number;
			int i = 0;
			this.Frames = this.ReadFrames(line, ref i);
			if (this.Frames == 0)
			{
				return;
			}
			while (i < line.Length)
			{
				char c = char.ToUpper(line[i]);
				if (c <= 'J')
				{
					switch (c)
					{
					case 'A':
						this.Actions ^= Actions.Angle;
						i++;
						this.Angle = this.ReadAngle(line, ref i);
						continue;
					case 'B':
						this.Actions ^= Actions.Bouncy;
						break;
					case 'C':
					case 'E':
					case 'F':
						break;
					case 'D':
						this.Actions ^= Actions.Down;
						break;
					case 'G':
						this.Actions ^= Actions.Goo;
						break;
					default:
						if (c == 'J')
						{
							this.Actions ^= Actions.Jump;
						}
						break;
					}
				}
				else if (c != 'L')
				{
					switch (c)
					{
					case 'R':
						this.Actions ^= Actions.Right;
						break;
					case 'S':
						this.Actions ^= Actions.Start;
						break;
					case 'U':
						this.Actions ^= Actions.Up;
						break;
					case 'W':
						this.Actions ^= Actions.Water;
						break;
					case 'X':
						this.Actions ^= Actions.Select;
						break;
					case '[':
						this.Actions ^= Actions.LeftBumper;
						break;
					case ']':
						this.Actions ^= Actions.RightBumper;
						break;
					}
				}
				else
				{
					this.Actions ^= Actions.Left;
				}
				i++;
			}
			if (this.HasActions(Actions.Angle))
			{
				this.Actions &= ~(Actions.Left | Actions.Right | Actions.Up | Actions.Down);
			}
			else
			{
				this.Angle = 0f;
			}
			if (this.HasActions(Actions.Bouncy))
			{
				this.Actions &= ~(Actions.Water | Actions.Goo);
				return;
			}
			if (this.HasActions(Actions.Water))
			{
				this.Actions &= ~Actions.Goo;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002920 File Offset: 0x00000B20
		private int ReadFrames(string line, ref int start)
		{
			bool flag = false;
			int num = 0;
			while (start < line.Length)
			{
				char c = line[start];
				if (!flag)
				{
					if (char.IsDigit(c))
					{
						flag = true;
						num = (int)(c ^ '0');
					}
					else if (c != ' ')
					{
						return num;
					}
				}
				else if (char.IsDigit(c))
				{
					if (num < 9999)
					{
						num = num * 10 + (int)(c ^ '0');
					}
					else
					{
						num = 9999;
					}
				}
				else if (c != ' ')
				{
					return num;
				}
				start++;
			}
			return num;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002994 File Offset: 0x00000B94
		private float ReadAngle(string line, ref int start)
		{
			bool flag = false;
			bool flag2 = false;
			int num = 1;
			int num2 = 0;
			bool flag3 = false;
			while (start < line.Length)
			{
				char c = line[start];
				if (!flag)
				{
					if (char.IsDigit(c))
					{
						flag = true;
						num2 = (int)(c ^ '0');
					}
					else if (c == '.')
					{
						flag = true;
						flag2 = true;
					}
					else if (c == '-')
					{
						flag3 = true;
					}
				}
				else if (char.IsDigit(c))
				{
					num2 = num2 * 10 + (int)(c ^ '0');
					if (flag2)
					{
						num *= 10;
					}
				}
				else if (c == '.')
				{
					flag2 = true;
				}
				else if (c != ' ')
				{
					return (flag3 ? ((float)(-(float)num2)) : ((float)num2)) / (float)num;
				}
				start++;
			}
			return (flag3 ? ((float)(-(float)num2)) : ((float)num2)) / (float)num;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002A44 File Offset: 0x00000C44
		public float GetX()
		{
			if (this.HasActions(Actions.Right))
			{
				return 1f;
			}
			if (this.HasActions(Actions.Left))
			{
				return -1f;
			}
			if (!this.HasActions(Actions.Angle))
			{
				return 0f;
			}
			float num = (float)Math.Sin((double)this.Angle * 3.1415926535897931 / 180.0);
			if (Math.Abs(num) < 0.1f)
			{
				return 0f;
			}
			return num;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public float GetXMax()
		{
			float x = this.GetX();
			if (x < -0.1f)
			{
				return -1f;
			}
			if (x > 0.1f)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002AF0 File Offset: 0x00000CF0
		public float GetY()
		{
			if (this.HasActions(Actions.Up))
			{
				return 1f;
			}
			if (this.HasActions(Actions.Down))
			{
				return -1f;
			}
			if (!this.HasActions(Actions.Angle))
			{
				return 0f;
			}
			float num = (float)Math.Cos((double)this.Angle * 3.1415926535897931 / 180.0);
			if (Math.Abs(num) < 0.1f)
			{
				return 0f;
			}
			return num;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B64 File Offset: 0x00000D64
		public float GetYMax()
		{
			float y = this.GetY();
			if (y < -0.1f)
			{
				return -1f;
			}
			if (y > 0.1f)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002169 File Offset: 0x00000369
		public bool HasActions(Actions actions)
		{
			return (this.Actions & actions) > Actions.None;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002B9C File Offset: 0x00000D9C
		public override string ToString()
		{
			if (this.Frames != 0)
			{
				return this.Frames.ToString().PadLeft(4, ' ') + this.ActionsToString();
			}
			return string.Empty;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002BD8 File Offset: 0x00000DD8
		public string ActionsToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.HasActions(Actions.Left))
			{
				stringBuilder.Append(",L");
			}
			if (this.HasActions(Actions.Right))
			{
				stringBuilder.Append(",R");
			}
			if (this.HasActions(Actions.Up))
			{
				stringBuilder.Append(",U");
			}
			if (this.HasActions(Actions.Down))
			{
				stringBuilder.Append(",D");
			}
			if (this.HasActions(Actions.Jump))
			{
				stringBuilder.Append(",J");
			}
			if (this.HasActions(Actions.Water))
			{
				stringBuilder.Append(",W");
			}
			if (this.HasActions(Actions.Goo))
			{
				stringBuilder.Append(",G");
			}
			if (this.HasActions(Actions.Bouncy))
			{
				stringBuilder.Append(",B");
			}
			if (this.HasActions(Actions.Start))
			{
				stringBuilder.Append(",S");
			}
			if (this.HasActions(Actions.Select))
			{
				stringBuilder.Append(",X");
			}
			if (this.HasActions(Actions.LeftBumper))
			{
				stringBuilder.Append(",[");
			}
			if (this.HasActions(Actions.RightBumper))
			{
				stringBuilder.Append(",]");
			}
			if (this.HasActions(Actions.Angle))
			{
				stringBuilder.Append(",A,").Append(this.Angle.ToString("0"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002176 File Offset: 0x00000376
		public override bool Equals(object obj)
		{
			return obj is InputRecord && (InputRecord)obj == this;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000218E File Offset: 0x0000038E
		public override int GetHashCode()
		{
			return this.Frames ^ (int)this.Actions;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002D38 File Offset: 0x00000F38
		public static bool operator ==(InputRecord one, InputRecord two)
		{
			bool flag = one == null;
			bool flag2 = two == null;
			return flag == flag2 && ((flag && flag2) || (one.Actions == two.Actions && one.Angle == two.Angle));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002D7C File Offset: 0x00000F7C
		public static bool operator !=(InputRecord one, InputRecord two)
		{
			bool flag = one == null;
			bool flag2 = two == null;
			return flag != flag2 || ((!flag || !flag2) && (one.Actions != two.Actions || one.Angle != two.Angle));
		}
	}
}
