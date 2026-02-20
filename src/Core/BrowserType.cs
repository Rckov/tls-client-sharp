using Http.TLS.Core.Converters;

using System.Text.Json.Serialization;

namespace Http.TLS.Core;

/// <summary>
/// TLS client fingerprint for browser impersonation.
/// </summary>
[JsonConverter(typeof(BrowserTypeConverter))]
public sealed class BrowserType(string value)
{
	public static readonly BrowserType Chrome103 = new("chrome_103");
	public static readonly BrowserType Chrome104 = new("chrome_104");
	public static readonly BrowserType Chrome105 = new("chrome_105");
	public static readonly BrowserType Chrome106 = new("chrome_106");
	public static readonly BrowserType Chrome107 = new("chrome_107");
	public static readonly BrowserType Chrome108 = new("chrome_108");
	public static readonly BrowserType Chrome109 = new("chrome_109");
	public static readonly BrowserType Chrome110 = new("chrome_110");
	public static readonly BrowserType Chrome111 = new("chrome_111");
	public static readonly BrowserType Chrome112 = new("chrome_112");
	public static readonly BrowserType Chrome116Psk = new("chrome_116_PSK");
	public static readonly BrowserType Chrome116PskPq = new("chrome_116_PSK_PQ");
	public static readonly BrowserType Chrome117 = new("chrome_117");
	public static readonly BrowserType Chrome120 = new("chrome_120");
	public static readonly BrowserType Chrome124 = new("chrome_124");
	public static readonly BrowserType Chrome130Psk = new("chrome_130_PSK");
	public static readonly BrowserType Chrome131 = new("chrome_131");
	public static readonly BrowserType Chrome131Psk = new("chrome_131_PSK");
	public static readonly BrowserType Chrome133 = new("chrome_133");
	public static readonly BrowserType Chrome133Psk = new("chrome_133_PSK");
	public static readonly BrowserType Chrome144 = new("chrome_144");
	public static readonly BrowserType Chrome144Psk = new("chrome_144_PSK");
	public static readonly BrowserType Chrome146 = new("chrome_146");
	public static readonly BrowserType Chrome146Psk = new("chrome_146_PSK");
	public static readonly BrowserType Safari1561 = new("safari_15_6_1");
	public static readonly BrowserType Safari160 = new("safari_16_0");
	public static readonly BrowserType SafariIpad156 = new("safari_ipad_15_6");
	public static readonly BrowserType SafariIos155 = new("safari_ios_15_5");
	public static readonly BrowserType SafariIos156 = new("safari_ios_15_6");
	public static readonly BrowserType SafariIos160 = new("safari_ios_16_0");
	public static readonly BrowserType SafariIos170 = new("safari_ios_17_0");
	public static readonly BrowserType SafariIos180 = new("safari_ios_18_0");
	public static readonly BrowserType SafariIos185 = new("safari_ios_18_5");
	public static readonly BrowserType SafariIos260 = new("safari_ios_26_0");
	public static readonly BrowserType Firefox102 = new("firefox_102");
	public static readonly BrowserType Firefox104 = new("firefox_104");
	public static readonly BrowserType Firefox105 = new("firefox_105");
	public static readonly BrowserType Firefox106 = new("firefox_106");
	public static readonly BrowserType Firefox108 = new("firefox_108");
	public static readonly BrowserType Firefox110 = new("firefox_110");
	public static readonly BrowserType Firefox117 = new("firefox_117");
	public static readonly BrowserType Firefox120 = new("firefox_120");
	public static readonly BrowserType Firefox123 = new("firefox_123");
	public static readonly BrowserType Firefox132 = new("firefox_132");
	public static readonly BrowserType Firefox133 = new("firefox_133");
	public static readonly BrowserType Firefox135 = new("firefox_135");
	public static readonly BrowserType Firefox146Psk = new("firefox_146_PSK");
	public static readonly BrowserType Firefox147 = new("firefox_147");
	public static readonly BrowserType Firefox147Psk = new("firefox_147_PSK");
	public static readonly BrowserType Opera89 = new("opera_89");
	public static readonly BrowserType Opera90 = new("opera_90");
	public static readonly BrowserType Opera91 = new("opera_91");
	public static readonly BrowserType MmsIos = new("mms_ios");
	public static readonly BrowserType MmsIos1 = new("mms_ios_1");
	public static readonly BrowserType MmsIos2 = new("mms_ios_2");
	public static readonly BrowserType MmsIos3 = new("mms_ios_3");
	public static readonly BrowserType MeshIos = new("mesh_ios");
	public static readonly BrowserType MeshIos1 = new("mesh_ios_1");
	public static readonly BrowserType MeshIos2 = new("mesh_ios_2");
	public static readonly BrowserType MeshAndroid = new("mesh_android");
	public static readonly BrowserType MeshAndroid1 = new("mesh_android_1");
	public static readonly BrowserType MeshAndroid2 = new("mesh_android_2");
	public static readonly BrowserType ConfirmedIos = new("confirmed_ios");
	public static readonly BrowserType ConfirmedAndroid = new("confirmed_android");
	public static readonly BrowserType Okhttp4Android7 = new("okhttp4_android_7");
	public static readonly BrowserType Okhttp4Android8 = new("okhttp4_android_8");
	public static readonly BrowserType Okhttp4Android9 = new("okhttp4_android_9");
	public static readonly BrowserType Okhttp4Android10 = new("okhttp4_android_10");
	public static readonly BrowserType Okhttp4Android11 = new("okhttp4_android_11");
	public static readonly BrowserType Okhttp4Android12 = new("okhttp4_android_12");
	public static readonly BrowserType Okhttp4Android13 = new("okhttp4_android_13");
	public static readonly BrowserType ZalandoAndroidMobile = new("zalando_android_mobile");
	public static readonly BrowserType ZalandoIosMobile = new("zalando_ios_mobile");
	public static readonly BrowserType NikeIosMobile = new("nike_ios_mobile");
	public static readonly BrowserType NikeAndroidMobile = new("nike_android_mobile");
	public static readonly BrowserType Cloudscraper = new("cloudscraper");

	public string Value { get; } = value;

	public override string ToString() => Value;
}