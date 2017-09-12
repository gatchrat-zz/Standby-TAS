using System;

namespace TAS
{
	// Token: 0x02000005 RID: 5
	[Flags]
	public enum State
	{
		// Token: 0x0400001B RID: 27
		None = 0,
		// Token: 0x0400001C RID: 28
		Enable = 1,
		// Token: 0x0400001D RID: 29
		Record = 2,
		// Token: 0x0400001E RID: 30
		FrameStep = 4
	}
}
