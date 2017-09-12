using System;

namespace TAS
{
	// Token: 0x02000003 RID: 3
	[Flags]
	public enum Actions
	{
		// Token: 0x04000008 RID: 8
		None = 0,
		// Token: 0x04000009 RID: 9
		Left = 1,
		// Token: 0x0400000A RID: 10
		Right = 2,
		// Token: 0x0400000B RID: 11
		Up = 4,
		// Token: 0x0400000C RID: 12
		Down = 8,
		// Token: 0x0400000D RID: 13
		Jump = 16,
		// Token: 0x0400000E RID: 14
		Water = 32,
		// Token: 0x0400000F RID: 15
		Goo = 64,
		// Token: 0x04000010 RID: 16
		Bouncy = 128,
		// Token: 0x04000011 RID: 17
		Start = 256,
		// Token: 0x04000012 RID: 18
		Select = 512,
		// Token: 0x04000013 RID: 19
		LeftBumper = 1024,
		// Token: 0x04000014 RID: 20
		RightBumper = 2048,
		// Token: 0x04000015 RID: 21
		Angle = 4096
	}
}
