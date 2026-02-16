using System.ComponentModel;

namespace Http.TLS.Core;

/// <summary>
/// TLS client fingerprints for browser impersonation.
/// </summary>
public enum BrowserType
{
	[Description("chrome_103")]
	Chrome103,

	[Description("chrome_104")]
	Chrome104,

	[Description("chrome_105")]
	Chrome105,

	[Description("chrome_106")]
	Chrome106,

	[Description("chrome_107")]
	Chrome107,

	[Description("chrome_108")]
	Chrome108,

	[Description("chrome_109")]
	Chrome109,

	[Description("chrome_110")]
	Chrome110,

	[Description("chrome_111")]
	Chrome111,

	[Description("chrome_112")]
	Chrome112,

	[Description("chrome_116_PSK")]
	Chrome116Psk,

	[Description("chrome_116_PSK_PQ")]
	Chrome116PskPq,

	[Description("chrome_117")]
	Chrome117,

	[Description("chrome_120")]
	Chrome120,

	[Description("chrome_124")]
	Chrome124,

	[Description("chrome_130_PSK")]
	Chrome130Psk,

	[Description("chrome_131")]
	Chrome131,

	[Description("chrome_131_PSK")]
	Chrome131Psk,

	[Description("chrome_133")]
	Chrome133,

	[Description("chrome_133_PSK")]
	Chrome133Psk,

	[Description("chrome_144")]
	Chrome144,

	[Description("chrome_144_PSK")]
	Chrome144Psk,

	[Description("chrome_146")]
	Chrome146,

	[Description("chrome_146_PSK")]
	Chrome146Psk,

	[Description("safari_15_6_1")]
	Safari1561,

	[Description("safari_16_0")]
	Safari160,

	[Description("safari_ipad_15_6")]
	SafariIpad156,

	[Description("safari_ios_15_5")]
	SafariIos155,

	[Description("safari_ios_15_6")]
	SafariIos156,

	[Description("safari_ios_16_0")]
	SafariIos160,

	[Description("safari_ios_17_0")]
	SafariIos170,

	[Description("safari_ios_18_0")]
	SafariIos180,

	[Description("safari_ios_18_5")]
	SafariIos185,

	[Description("safari_ios_26_0")]
	SafariIos260,

	[Description("firefox_102")]
	Firefox102,

	[Description("firefox_104")]
	Firefox104,

	[Description("firefox_105")]
	Firefox105,

	[Description("firefox_106")]
	Firefox106,

	[Description("firefox_108")]
	Firefox108,

	[Description("firefox_110")]
	Firefox110,

	[Description("firefox_117")]
	Firefox117,

	[Description("firefox_120")]
	Firefox120,

	[Description("firefox_123")]
	Firefox123,

	[Description("firefox_132")]
	Firefox132,

	[Description("firefox_133")]
	Firefox133,

	[Description("firefox_135")]
	Firefox135,

	[Description("firefox_146_PSK")]
	Firefox146Psk,

	[Description("firefox_147")]
	Firefox147,

	[Description("firefox_147_PSK")]
	Firefox147Psk,

	[Description("opera_89")]
	Opera89,

	[Description("opera_90")]
	Opera90,

	[Description("opera_91")]
	Opera91,

	[Description("mms_ios")]
	MmsIos,

	[Description("mms_ios_1")]
	MmsIos1,

	[Description("mms_ios_2")]
	MmsIos2,

	[Description("mms_ios_3")]
	MmsIos3,

	[Description("mesh_ios")]
	MeshIos,

	[Description("mesh_ios_1")]
	MeshIos1,

	[Description("mesh_ios_2")]
	MeshIos2,

	[Description("mesh_android")]
	MeshAndroid,

	[Description("mesh_android_1")]
	MeshAndroid1,

	[Description("mesh_android_2")]
	MeshAndroid2,

	[Description("confirmed_ios")]
	ConfirmedIos,

	[Description("confirmed_android")]
	ConfirmedAndroid,

	[Description("okhttp4_android_7")]
	Okhttp4Android7,

	[Description("okhttp4_android_8")]
	Okhttp4Android8,

	[Description("okhttp4_android_9")]
	Okhttp4Android9,

	[Description("okhttp4_android_10")]
	Okhttp4Android10,

	[Description("okhttp4_android_11")]
	Okhttp4Android11,

	[Description("okhttp4_android_12")]
	Okhttp4Android12,

	[Description("okhttp4_android_13")]
	Okhttp4Android13,

	[Description("zalando_android_mobile")]
	ZalandoAndroidMobile,

	[Description("zalando_ios_mobile")]
	ZalandoIosMobile,

	[Description("nike_ios_mobile")]
	NikeIosMobile,

	[Description("nike_android_mobile")]
	NikeAndroidMobile,

	[Description("cloudscraper")]
	Cloudscraper
}