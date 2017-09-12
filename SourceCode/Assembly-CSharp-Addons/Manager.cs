using System;
using System.Threading;
using J2i.Net.XInputWrapper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TAS
{
	// Token: 0x02000006 RID: 6
	public class Manager
	{
		// Token: 0x06000026 RID: 38 RVA: 0x0000219D File Offset: 0x0000039D
		static Manager()
		{
			XboxController.UpdateFrequency = 30;
			XboxController.StartPolling();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000021C5 File Offset: 0x000003C5
		public static void Main(string[] args)
		{
			Manager.controller.ReloadPlayback();
			Console.WriteLine("Done");
			Console.ReadLine();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000021E1 File Offset: 0x000003E1
		public static bool IsLoading()
		{
			SceneManager.GetActiveScene();
			return false;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002DC4 File Offset: 0x00000FC4
		public static void UpdateInputs()
		{
			Manager.HandleFrameRates();
			Manager.CheckControls();
			Manager.FrameStepping();
			if (Manager.HasFlag(Manager.state, State.Enable))
			{
				Manager.Running = true;
				if (Manager.HasFlag(Manager.state, State.Record))
				{
					Manager.controller.RecordPlayer();
				}
				else
				{
					Manager.controller.PlaybackPlayer();
					if (!Manager.controller.CanPlayback)
					{
						Manager.DisableRun();
					}
				}
				Manager.CurrentStatus = string.Concat(new object[]
				{
					Manager.controller.Current.Line,
					"[",
					Manager.controller.ToString(),
					"]"
				});
				return;
			}
			Manager.Running = false;
			Manager.CurrentStatus = null;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002E78 File Offset: 0x00001078
		public static float GetAxis(string axisName)
		{
			InputRecord inputRecord = Manager.controller.Current;
			if (axisName == "Horizontal")
			{
				return -inputRecord.GetX();
			}
			if (axisName == "Vertical")
			{
				return -inputRecord.GetY();
			}
			if (axisName == "DPadX")
			{
				return inputRecord.GetXMax();
			}
			if (axisName == "DPadY")
			{
				return inputRecord.GetYMax();
			}
			if (axisName == "LeftStickX")
			{
				return inputRecord.GetX();
			}
			if (!(axisName == "LeftStickY"))
			{
				return 0f;
			}
			return inputRecord.GetY();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002E78 File Offset: 0x00001078
		public static float GetAxisRaw(string axisName)
		{
			InputRecord inputRecord = Manager.controller.Current;
			if (axisName == "Horizontal")
			{
				return -inputRecord.GetX();
			}
			if (axisName == "Vertical")
			{
				return -inputRecord.GetY();
			}
			if (axisName == "DPadX")
			{
				return inputRecord.GetXMax();
			}
			if (axisName == "DPadY")
			{
				return inputRecord.GetYMax();
			}
			if (axisName == "LeftStickX")
			{
				return inputRecord.GetX();
			}
			if (!(axisName == "LeftStickY"))
			{
				return 0f;
			}
			return inputRecord.GetY();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002F10 File Offset: 0x00001110
		public static int GetAxisDown(string AxisName)
		{
			InputRecord inputRecord = Manager.controller.Current;
			if (AxisName == "Horizontal")
			{
				return -(int)inputRecord.GetXMax();
			}
			if (AxisName == "Vertical")
			{
				return -(int)inputRecord.GetYMax();
			}
			if (AxisName == "DPadX")
			{
				return (int)inputRecord.GetXMax();
			}
			if (AxisName == "DPadY")
			{
				return (int)inputRecord.GetYMax();
			}
			if (AxisName == "LeftStickX")
			{
				return (int)inputRecord.GetXMax();
			}
			if (!(AxisName == "LeftStickY"))
			{
				return 0;
			}
			return (int)inputRecord.GetYMax();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002FAC File Offset: 0x000011AC
		private static void HandleFrameRates()
		{
			if (!Manager.HasFlag(Manager.state, State.Enable) || Manager.HasFlag(Manager.state, State.FrameStep) || Manager.HasFlag(Manager.state, State.Record))
			{
				Manager.SetFrameRate(60);
				return;
			}
			float num = (float)Manager.xbox.RightThumbStickX / 32768f;
			if ((double)num <= -0.9)
			{
				Manager.SetFrameRate(3);
				return;
			}
			if ((double)num <= -0.8)
			{
				Manager.SetFrameRate(6);
				return;
			}
			if ((double)num <= -0.7)
			{
				Manager.SetFrameRate(12);
				return;
			}
			if ((double)num <= -0.6)
			{
				Manager.SetFrameRate(16);
				return;
			}
			if ((double)num <= -0.5)
			{
				Manager.SetFrameRate(20);
				return;
			}
			if ((double)num <= -0.4)
			{
				Manager.SetFrameRate(28);
				return;
			}
			if ((double)num <= -0.3)
			{
				Manager.SetFrameRate(36);
				return;
			}
			if ((double)num <= -0.2)
			{
				Manager.SetFrameRate(44);
				return;
			}
			if ((double)num <= 0.2)
			{
				Manager.SetFrameRate(60);
				return;
			}
			if ((double)num <= 0.3)
			{
				Manager.SetFrameRate(80);
				return;
			}
			if ((double)num <= 0.4)
			{
				Manager.SetFrameRate(100);
				return;
			}
			if ((double)num <= 0.5)
			{
				Manager.SetFrameRate(120);
				return;
			}
			if ((double)num <= 0.6)
			{
				Manager.SetFrameRate(140);
				return;
			}
			if ((double)num <= 0.7)
			{
				Manager.SetFrameRate(160);
				return;
			}
			if ((double)num <= 0.8)
			{
				Manager.SetFrameRate(180);
				return;
			}
			if ((double)num <= 0.9)
			{
				Manager.SetFrameRate(200);
				return;
			}
			Manager.SetFrameRate(240);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000021EA File Offset: 0x000003EA
		private static void SetFrameRate(int newFrameRate = 60)
		{
			if (Manager.frameRate == newFrameRate)
			{
				return;
			}
			Manager.frameRate = newFrameRate;
			Time.timeScale = (float)newFrameRate / 60f;
			Time.captureFramerate = newFrameRate;
			Application.targetFrameRate = newFrameRate;
			QualitySettings.vSyncCount = ((newFrameRate == 60) ? 1 : 0);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003160 File Offset: 0x00001360
		private static void FrameStepping()
		{
			float num = (float)Manager.xbox.RightThumbStickX / 32768f;
			bool flag = Manager.xbox.RightTrigger == 255;
			bool isDPadUpPressed = Manager.xbox.IsDPadUpPressed;
			bool isDPadDownPressed = Manager.xbox.IsDPadDownPressed;
			if (Manager.HasFlag(Manager.state, State.Enable) && !Manager.HasFlag(Manager.state, State.Record) && (Manager.HasFlag(Manager.state, State.FrameStep) || (isDPadUpPressed && !flag)))
			{
				bool flag2 = isDPadUpPressed;
				while (Manager.HasFlag(Manager.state, State.Enable))
				{
					num = (float)Manager.xbox.RightThumbStickX / 32768f;
					flag = (Manager.xbox.RightTrigger == 255);
					isDPadUpPressed = Manager.xbox.IsDPadUpPressed;
					isDPadDownPressed = Manager.xbox.IsDPadDownPressed;
					Manager.CheckControls();
					if (!flag2 && isDPadUpPressed && !flag)
					{
						Manager.state |= State.FrameStep;
						break;
					}
					if (isDPadDownPressed && !flag)
					{
						Manager.state &= ~State.FrameStep;
						break;
					}
					if ((double)num >= 0.2)
					{
						Manager.state |= State.FrameStep;
						int millisecondsTimeout = 0;
						if ((double)num <= 0.3)
						{
							millisecondsTimeout = 200;
						}
						else if ((double)num <= 0.4)
						{
							millisecondsTimeout = 100;
						}
						else if ((double)num <= 0.5)
						{
							millisecondsTimeout = 80;
						}
						else if ((double)num <= 0.6)
						{
							millisecondsTimeout = 64;
						}
						else if ((double)num <= 0.7)
						{
							millisecondsTimeout = 48;
						}
						else if ((double)num <= 0.8)
						{
							millisecondsTimeout = 32;
						}
						else if ((double)num <= 0.9)
						{
							millisecondsTimeout = 16;
						}
						Thread.Sleep(millisecondsTimeout);
						break;
					}
					flag2 = isDPadUpPressed;
					Thread.Sleep(1);
				}
				Manager.ReloadRun();
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003328 File Offset: 0x00001528
		private static void CheckControls()
		{
			bool isLeftStickPressed = Manager.xbox.IsLeftStickPressed;
			bool isRightStickPressed = Manager.xbox.IsRightStickPressed;
			bool flag = Manager.xbox.RightTrigger >= 245;
			bool flag2 = Manager.xbox.LeftTrigger >= 245;
			bool isDPadDownPressed = Manager.xbox.IsDPadDownPressed;
			if (flag && flag2)
			{
				if (!Manager.HasFlag(Manager.state, State.Enable) && isRightStickPressed)
				{
					Manager.nextState |= State.Enable;
				}
				else if (Manager.HasFlag(Manager.state, State.Enable) && isDPadDownPressed)
				{
					Manager.DisableRun();
				}
				else if (!Manager.HasFlag(Manager.state, State.Enable) && !Manager.HasFlag(Manager.state, State.Record) && isLeftStickPressed)
				{
					Manager.nextState |= State.Record;
				}
			}
			if (!flag && !flag2)
			{
				if (Manager.HasFlag(Manager.nextState, State.Enable))
				{
					Manager.EnableRun();
					return;
				}
				if (Manager.HasFlag(Manager.nextState, State.Record))
				{
					Manager.RecordRun();
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003414 File Offset: 0x00001614
		public static void GetCurrentInputs(InputRecord record)
		{
			float axisRaw = Input.GetAxisRaw("Horizontal");
			if (axisRaw > 0f)
			{
				record.Actions |= Actions.Right;
			}
			else if (axisRaw < 0f)
			{
				record.Actions |= Actions.Left;
			}
			if (Input.GetKey(KeyCode.JoystickButton2))
			{
				record.Actions |= Actions.Water;
			}
			if (Input.GetKey(KeyCode.JoystickButton0))
			{
				record.Actions |= Actions.Jump;
			}
			if (Input.GetKey(KeyCode.JoystickButton7))
			{
				record.Actions |= Actions.Start;
			}
			if (Input.GetKey(KeyCode.JoystickButton6))
			{
				record.Actions |= Actions.Select;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002222 File Offset: 0x00000422
		private static float SetToMax(float value, float min, float max)
		{
			if (value < -min)
			{
				return -max;
			}
			if (value > min)
			{
				return max;
			}
			return 0f;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002237 File Offset: 0x00000437
		private static void DisableRun()
		{
			Manager.Running = false;
			Manager.Recording = false;
			Manager.state &= ~State.Enable;
			Manager.state &= ~State.FrameStep;
			Manager.state &= ~State.Record;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000226C File Offset: 0x0000046C
		private static void EnableRun()
		{
			Manager.nextState &= ~State.Enable;
			Manager.UpdateVariables(false);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002281 File Offset: 0x00000481
		private static void RecordRun()
		{
			Manager.nextState &= ~State.Record;
			Manager.UpdateVariables(true);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002296 File Offset: 0x00000496
		private static void ReloadRun()
		{
			Manager.controller.ReloadPlayback();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000034CC File Offset: 0x000016CC
		private static void UpdateVariables(bool recording)
		{
			Manager.state |= State.Enable;
			Manager.state &= ~State.FrameStep;
			if (recording)
			{
				Manager.Recording = recording;
				Manager.state |= State.Record;
				Manager.controller.InitializeRecording();
			}
			else
			{
				Manager.state &= ~State.Record;
				Manager.controller.InitializePlayback();
			}
			Manager.Running = true;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000022A2 File Offset: 0x000004A2
		private static bool HasFlag(State state, State flag)
		{
			return (state & flag) == flag;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003530 File Offset: 0x00001730
		public static bool GetButtonDown(int button)
		{
			if (Manager.controller.CurrentInputFrame != 1)
			{
				return false;
			}
			InputRecord inputRecord = Manager.controller.Current;
			switch (button)
			{
			case 0:
				return inputRecord.HasActions(Actions.Jump);
			case 1:
				return inputRecord.HasActions(Actions.Goo);
			case 2:
				return inputRecord.HasActions(Actions.Bouncy);
			case 3:
				return inputRecord.HasActions(Actions.Water);
			case 4:
				return inputRecord.HasActions(Actions.Start);
			case 5:
				return inputRecord.HasActions(Actions.Select);
			case 6:
				return inputRecord.HasActions(Actions.LeftBumper);
			case 7:
				return inputRecord.HasActions(Actions.RightBumper);
			default:
				return false;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000035D8 File Offset: 0x000017D8
		public static bool GetButton(int button)
		{
			InputRecord inputRecord = Manager.controller.Current;
			switch (button)
			{
			case 0:
				return inputRecord.HasActions(Actions.Jump);
			case 1:
				return inputRecord.HasActions(Actions.Goo);
			case 2:
				return inputRecord.HasActions(Actions.Bouncy);
			case 3:
				return inputRecord.HasActions(Actions.Water);
			case 4:
				return inputRecord.HasActions(Actions.Start);
			case 5:
				return inputRecord.HasActions(Actions.Select);
			case 6:
				return inputRecord.HasActions(Actions.LeftBumper);
			case 7:
				return inputRecord.HasActions(Actions.RightBumper);
			default:
				return false;
			}
		}

		// Token: 0x0400001F RID: 31
		public static bool Running;

		// Token: 0x04000020 RID: 32
		public static bool Recording;

		// Token: 0x04000021 RID: 33
		private static InputController controller = new InputController("Standby.tas");

		// Token: 0x04000022 RID: 34
		private static State state;

		// Token: 0x04000023 RID: 35
		private static State nextState;

		// Token: 0x04000024 RID: 36
		private static int frameRate;

		// Token: 0x04000025 RID: 37
		private static XboxController xbox = XboxController.RetrieveController(0);

		// Token: 0x04000026 RID: 38
		public static string CurrentStatus;

		// Token: 0x04000027 RID: 39
		public static string NextScene;

		// Token: 0x04000028 RID: 40
		private static float deltaTime;
	}
}
