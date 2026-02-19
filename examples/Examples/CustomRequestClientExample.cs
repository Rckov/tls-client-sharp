using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Http.TLS.Examples.Examples;

public sealed class CustomRequestClientExample(IRequestClientFactory factory) : IExample
{
	private static readonly int[] GreaseValues =
	[
		0x0A0A, 0x1A1A, 0x2A2A, 0x3A3A, 0x4A4A, 0x5A5A, 0x6A6A, 0x7A7A,
		0x8A8A, 0x9A9A, 0xAAAA, 0xBABA, 0xCACA, 0xDADA, 0xEAEA, 0xFAFA
	];

	public async Task RunAsync()
	{
		using var client = factory.CreateClient("custom-tls");

		var request = new RequestBuilder()
			.WithUrl("https://tls.peet.ws/api/all")
			.Build();

		var response = await client.SendAsync(request);
		PrintResponse(response);
	}

	private static void PrintResponse(Response? response)
	{
		if (response is null)
		{
			return;
		}

		Console.WriteLine($"Status: {response.Status}");
		Console.WriteLine($"Body: {response.Body}");
	}

	public static CustomRequestClient BuildProfile()
	{
		var grease = GreaseValues[Random.Shared.Next(GreaseValues.Length)];
		var ja3 = BuildJa3Fingerprint(grease);

		return BuildCustomProfile(ja3);
	}

	private static string BuildJa3Fingerprint(int grease)
	{
		var cipherSuites = GetCipherSuites(grease);
		var extensions = GetExtensions(grease);
		var ellipticCurves = GetEllipticCurves(grease);
		var pointFormats = new[] { 0 };

		return BuildJa3String(771, cipherSuites, extensions, ellipticCurves, pointFormats);
	}

	private static int[] GetCipherSuites(int grease)
	{
		return
		[
			grease,
			0x1301, // TLS_AES_128_GCM_SHA256
			0x1302, // TLS_AES_256_GCM_SHA384
			0x1303, // TLS_CHACHA20_POLY1305_SHA256
			0xC02B, // TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256
			0xC02F, // TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256
			0xC02C, // TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384
			0xC030, // TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384
			0xCCA9, // TLS_ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256
			0xCCA8, // TLS_ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256
			0xC013, // TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA
			0xC014, // TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA
			0x009C, // TLS_RSA_WITH_AES_128_GCM_SHA256
			0x009D, // TLS_RSA_WITH_AES_256_GCM_SHA384
			0x002F, // TLS_RSA_WITH_AES_128_CBC_SHA
			0x0035  // TLS_RSA_WITH_AES_256_CBC_SHA
		];
	}

	private static int[] GetExtensions(int grease)
	{
		return
		[
			grease, // UtlsGREASEExtension
			17613,  // application_settings
			43,     // supported_versions
			18,     // signed_certificate_timestamp
			65037,  // encrypted_client_hello
			51,     // key_share
			13,     // signature_algorithms
			10,     // supported_groups
			27,     // compress_certificate
			23,     // extended_master_secret
			35,     // session_ticket
			0,      // server_name
			65281,  // renegotiation_info
			45,     // psk_key_exchange_modes
			11,     // ec_point_formats
			5,      // status_request
			16,     // application_layer_protocol_negotiation
			grease  // UtlsGREASEExtension
		];
	}

	private static int[] GetEllipticCurves(int grease)
	{
		return
		[
			grease,
			4588,   // X25519MLKEM768
			29,     // X25519
			23,     // secp256r1 (P-256)
			24      // secp384r1 (P-384)
		];
	}

	private static CustomRequestClient BuildCustomProfile(string ja3Fingerprint)
	{
		return new CustomRequestClient
		{
			Ja3Fingerprint = ja3Fingerprint,

			Http2Settings = new Dictionary<string, uint>
			{
				["HEADER_TABLE_SIZE"] = 65536,
				["ENABLE_PUSH"] = 0,
				["INITIAL_WINDOW_SIZE"] = 6291456,
				["MAX_HEADER_LIST_SIZE"] = 262144
			},
			Http2SettingsOrder =
			[
				"HEADER_TABLE_SIZE",
				"ENABLE_PUSH",
				"INITIAL_WINDOW_SIZE",
				"MAX_HEADER_LIST_SIZE"
			],

			Http3Settings = new Dictionary<string, ulong>
			{
				["SETTINGS_QPACK_MAX_TABLE_CAPACITY"] = 65536,
				["SETTINGS_QPACK_BLOCKED_STREAMS"] = 100
			},
			Http3SettingsOrder =
			[
				"SETTINGS_QPACK_MAX_TABLE_CAPACITY",
				"SETTINGS_MAX_FIELD_SECTION_SIZE",
				"SETTINGS_QPACK_BLOCKED_STREAMS",
				"SETTINGS_H3_DATAGRAM"
			],

			CertificateCompressionAlgorithms = ["brotli"],
			KeyShareCurves = ["GREASE", "X25519MLKEM768", "X25519"],
			AlpnProtocols = ["h2", "http/1.1"],
			PseudoHeaderOrder = [":method", ":authority", ":scheme", ":path"],

			SupportedSignatureAlgorithms =
			[
				"ECDSAWithP256AndSHA256",
				"PSSWithSHA256",
				"PKCS1WithSHA256",
				"ECDSAWithP384AndSHA384",
				"PSSWithSHA384",
				"PKCS1WithSHA384",
				"PSSWithSHA512",
				"PKCS1WithSHA512"
			],
			SupportedVersions = ["GREASE", "1.3", "1.2"],

			ConnectionFlow = 15663105,
			RecordSizeLimit = 0,
			HeaderPriority = null,

			EchCandidatePayloads = [],
			EchCandidateCipherSuites = [],
			PriorityFrames = [],
			SupportedDelegatedCredentialsAlgorithms = []
		};
	}

	private static string BuildJa3String(
		int sslVersion,
		int[] cipherSuites,
		int[] extensions,
		int[] ellipticCurves,
		int[] ellipticCurvePointFormats)
	{
		var greaseSet = new HashSet<int>(GreaseValues);

		var ciphers = string.Join("-", cipherSuites);
		var exts = string.Join("-", extensions.Where(e => !greaseSet.Contains(e)));
		var curves = string.Join("-", ellipticCurves.Where(c => !greaseSet.Contains(c)));
		var pointFormats = string.Join("-", ellipticCurvePointFormats);

		return $"{sslVersion},{ciphers},{exts},{curves},{pointFormats}";
	}
}