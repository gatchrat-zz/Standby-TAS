using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TAS
{
	// Token: 0x02000002 RID: 2
	public class InputController
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public InputController(string filePath)
		{
			this.filePath = filePath;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000206A File Offset: 0x0000026A
		public bool CanPlayback
		{
			get
			{
				return this.inputIndex < this.inputs.Count;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000207F File Offset: 0x0000027F
		public int CurrentFrame
		{
			get
			{
				return this.currentFrame;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002087 File Offset: 0x00000287
		public int CurrentInputFrame
		{
			get
			{
				return this.currentFrame - this.frameToNext + this.Current.Frames;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020A2 File Offset: 0x000002A2
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020AA File Offset: 0x000002AA
		public InputRecord Current { get; set; }

		// Token: 0x06000007 RID: 7 RVA: 0x000022AC File Offset: 0x000004AC
		public override string ToString()
		{
			if (this.frameToNext == 0 && this.Current != null)
			{
				return this.Current.ToString() + "(" + this.currentFrame.ToString() + ")";
			}
			if (this.inputIndex < this.inputs.Count && this.Current != null)
			{
				int frames = this.Current.Frames;
				int num = this.frameToNext - frames;
				return string.Concat(new object[]
				{
					this.Current.ToString(),
					"(",
					(this.currentFrame - num).ToString(),
					" / ",
					frames,
					" : ",
					this.currentFrame,
					")"
				});
			}
			return string.Empty;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020B3 File Offset: 0x000002B3
		public string NextInput()
		{
			if (this.frameToNext != 0 && this.inputIndex + 1 < this.inputs.Count)
			{
				return this.inputs[this.inputIndex + 1].ToString();
			}
			return string.Empty;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002398 File Offset: 0x00000598
		public void InitializePlayback()
		{
			this.ReadFile();
			this.currentFrame = 0;
			this.inputIndex = 0;
			if (this.inputs.Count > 0)
			{
				this.Current = this.inputs[0];
				this.frameToNext = this.Current.Frames;
				return;
			}
			this.Current = new InputRecord();
			this.frameToNext = 1;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002400 File Offset: 0x00000600
		public void ReloadPlayback()
		{
			int num = this.currentFrame;
			this.InitializePlayback();
			this.currentFrame = num;
			while (this.currentFrame >= this.frameToNext)
			{
				if (this.inputIndex + 1 >= this.inputs.Count)
				{
					this.inputIndex++;
					return;
				}
				List<InputRecord> list = this.inputs;
				int index = this.inputIndex + 1;
				this.inputIndex = index;
				this.Current = list[index];
				this.frameToNext += this.Current.Frames;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000020F0 File Offset: 0x000002F0
		public void InitializeRecording()
		{
			this.currentFrame = 0;
			this.inputIndex = 0;
			this.Current = new InputRecord();
			this.frameToNext = 0;
			this.inputs.Clear();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002490 File Offset: 0x00000690
		public void PlaybackPlayer()
		{
			if (this.inputIndex < this.inputs.Count && !Manager.IsLoading())
			{
				if (this.currentFrame >= this.frameToNext)
				{
					if (this.inputIndex + 1 >= this.inputs.Count)
					{
						this.inputIndex++;
						return;
					}
					List<InputRecord> list = this.inputs;
					int index = this.inputIndex + 1;
					this.inputIndex = index;
					this.Current = list[index];
					this.frameToNext += this.Current.Frames;
				}
				this.currentFrame++;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002534 File Offset: 0x00000734
		public void RecordPlayer()
		{
			InputRecord inputRecord = new InputRecord
			{
				Line = this.inputIndex + 1,
				Frames = this.currentFrame
			};
			Manager.GetCurrentInputs(inputRecord);
			if (this.currentFrame == 0 && inputRecord == this.Current)
			{
				return;
			}
			if (inputRecord != this.Current && !Manager.IsLoading())
			{
				this.Current.Frames = this.currentFrame - this.Current.Frames;
				this.inputIndex++;
				if (this.Current.Frames != 0)
				{
					byte[] bytes = Encoding.ASCII.GetBytes(this.Current.ToString() + "\r\n");
					using (FileStream fileStream = new FileStream(this.filePath, (this.inputIndex == 1) ? FileMode.Create : FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
					{
						fileStream.Position = fileStream.Length;
						fileStream.Write(bytes, 0, bytes.Length);
						fileStream.Close();
					}
				}
				this.Current = inputRecord;
			}
			this.currentFrame++;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000265C File Offset: 0x0000085C
		private void ReadFile()
		{
			this.inputs.Clear();
			if (!File.Exists(this.filePath))
			{
				return;
			}
			int num = 0;
			using (StreamReader streamReader = new StreamReader(this.filePath))
			{
				while (!streamReader.EndOfStream)
				{
					string line = streamReader.ReadLine();
					InputRecord inputRecord = new InputRecord(++num, line);
					if (inputRecord.Frames != 0)
					{
						this.inputs.Add(inputRecord);
					}
				}
			}
		}

		// Token: 0x04000001 RID: 1
		private List<InputRecord> inputs = new List<InputRecord>();

		// Token: 0x04000002 RID: 2
		private int currentFrame;

		// Token: 0x04000003 RID: 3
		private int inputIndex;

		// Token: 0x04000004 RID: 4
		private int frameToNext;

		// Token: 0x04000005 RID: 5
		private string filePath;
	}
}
