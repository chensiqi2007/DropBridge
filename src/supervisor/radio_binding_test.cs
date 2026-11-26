// Q71: offline checks for RadioBinding multi-adapter matching.
using System;

internal static class RadioBindingTest
{
    public static int Main()
    {
        int failures = 0;

        RadioTarget c820 = RadioBinding.MatchTarget(@"USB\VID_0BDA&PID_C820&MI_02\7&123456&0&0002");
        Check(ref failures, c820 != null && c820.PidHex == "0xc820" && c820.Name == "RTL8821CU",
            "8821CU MI_02 matches");

        Check(ref failures, RadioBinding.MatchTarget(@"USB\VID_0BDA&PID_C820&MI_00\7&123456&0&0000") == null,
            "8821CU MI_00 Bluetooth must not match");
        Check(ref failures, RadioBinding.MatchTarget(@"USB\VID_0BDA&PID_C820\123456") == null,
            "8821CU composite parent must not match");

        RadioTarget c811 = RadioBinding.MatchTarget(@"usb\vid_0bda&pid_c811\123456");
        Check(ref failures, c811 != null && c811.PidHex == "0xc811" && c811.Name == "RTL8811CU",
            "8811CU non-composite matches case-insensitively");

        Check(ref failures, RadioBinding.MatchTarget(@"USB\VID_0BDA&PID_C811&MI_00\123456") == null,
            "8811CU MI_00 must not match non-composite rule");
        Check(ref failures, RadioBinding.MatchTarget(@"USB\VID_0BDA&PID_1A2B\123456") == null,
            "ZeroCD 1A2B must not match");
        Check(ref failures, RadioBinding.MatchTarget(null) == null, "null rejected");
        Check(ref failures, RadioBinding.MatchTarget("") == null, "empty rejected");

        Check(ref failures, RadioBinding.Bound("WinUSB"), "WinUSB bound");
        Check(ref failures, RadioBinding.Bound("winusb"), "winusb case-insensitive");
        Check(ref failures, !RadioBinding.Bound("RtlWlanu"), "RtlWlanu not bound");
        Check(ref failures, !RadioBinding.Bound(""), "empty service not bound");

        Console.WriteLine(failures == 0 ? "PASS radio_binding" : "FAIL radio_binding (" + failures + " case(s))");
        return failures == 0 ? 0 : 1;
    }

    private static void Check(ref int failures, bool ok, string what)
    {
        if (ok) return;
        failures++;
        Console.Error.WriteLine("FAIL: " + what);
    }
}
