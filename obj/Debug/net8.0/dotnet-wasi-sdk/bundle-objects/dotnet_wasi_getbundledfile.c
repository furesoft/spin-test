#include <string.h>

int mono_wasm_add_assembly(const char* name, const unsigned char* data, unsigned int size);

extern const unsigned char DotnetTest_dll_91A0EDE9[];
extern const int DotnetTest_dll_91A0EDE9_len;
extern const unsigned char System_Collections_dll_3E425A67[];
extern const int System_Collections_dll_3E425A67_len;
extern const unsigned char System_Private_CoreLib_dll_F87B835A[];
extern const int System_Private_CoreLib_dll_F87B835A_len;
extern const unsigned char System_Net_Primitives_dll_B35F26C2[];
extern const int System_Net_Primitives_dll_B35F26C2_len;
extern const unsigned char Microsoft_Win32_Primitives_dll_32937B25[];
extern const int Microsoft_Win32_Primitives_dll_32937B25_len;
extern const unsigned char System_Collections_NonGeneric_dll_828334FC[];
extern const int System_Collections_NonGeneric_dll_828334FC_len;
extern const unsigned char System_Threading_dll_36888B71[];
extern const int System_Threading_dll_36888B71_len;
extern const unsigned char System_Runtime_dll_19ED86F4[];
extern const int System_Runtime_dll_19ED86F4_len;
extern const unsigned char System_Private_Uri_dll_A6A36638[];
extern const int System_Private_Uri_dll_A6A36638_len;
extern const unsigned char System_Diagnostics_Tracing_dll_08CB5321[];
extern const int System_Diagnostics_Tracing_dll_08CB5321_len;
extern const unsigned char System_Memory_dll_5E9BA1A6[];
extern const int System_Memory_dll_5E9BA1A6_len;
extern const unsigned char System_Runtime_InteropServices_dll_C9CC0A41[];
extern const int System_Runtime_InteropServices_dll_C9CC0A41_len;
extern const unsigned char Fermyon_Spin_Sdk_dll_A4A336FA[];
extern const int Fermyon_Spin_Sdk_dll_A4A336FA_len;
extern const unsigned char System_Net_Http_dll_BD4445D7[];
extern const int System_Net_Http_dll_BD4445D7_len;
extern const unsigned char System_Private_Runtime_InteropServices_JavaScript_dll_F1F71C0B[];
extern const int System_Private_Runtime_InteropServices_JavaScript_dll_F1F71C0B_len;
extern const unsigned char System_Net_Security_dll_BF6427BB[];
extern const int System_Net_Security_dll_BF6427BB_len;
extern const unsigned char System_Security_Cryptography_dll_79859BDF[];
extern const int System_Security_Cryptography_dll_79859BDF_len;
extern const unsigned char System_Text_Encoding_Extensions_dll_E5113A13[];
extern const int System_Text_Encoding_Extensions_dll_E5113A13_len;
extern const unsigned char System_Collections_Concurrent_dll_60ED355F[];
extern const int System_Collections_Concurrent_dll_60ED355F_len;
extern const unsigned char System_Runtime_Numerics_dll_98BA933F[];
extern const int System_Runtime_Numerics_dll_98BA933F_len;
extern const unsigned char System_Formats_Asn1_dll_3A51E30A[];
extern const int System_Formats_Asn1_dll_3A51E30A_len;
extern const unsigned char System_Diagnostics_DiagnosticSource_dll_DDB8C4E1[];
extern const int System_Diagnostics_DiagnosticSource_dll_DDB8C4E1_len;
extern const unsigned char System_Threading_Thread_dll_477DC647[];
extern const int System_Threading_Thread_dll_477DC647_len;
extern const unsigned char System_Collections_Immutable_dll_8C4396FF[];
extern const int System_Collections_Immutable_dll_8C4396FF_len;
extern const unsigned char System_Linq_dll_00E030BE[];
extern const int System_Linq_dll_00E030BE_len;
extern const unsigned char System_Numerics_Vectors_dll_941BE9F4[];
extern const int System_Numerics_Vectors_dll_941BE9F4_len;

const unsigned char* dotnet_wasi_getbundledfile(const char* name, int* out_length) {
  return NULL;
}

void dotnet_wasi_registerbundledassemblies() {
  mono_wasm_add_assembly ("DotnetTest.dll", DotnetTest_dll_91A0EDE9, DotnetTest_dll_91A0EDE9_len);
  mono_wasm_add_assembly ("System.Collections.dll", System_Collections_dll_3E425A67, System_Collections_dll_3E425A67_len);
  mono_wasm_add_assembly ("System.Private.CoreLib.dll", System_Private_CoreLib_dll_F87B835A, System_Private_CoreLib_dll_F87B835A_len);
  mono_wasm_add_assembly ("System.Net.Primitives.dll", System_Net_Primitives_dll_B35F26C2, System_Net_Primitives_dll_B35F26C2_len);
  mono_wasm_add_assembly ("Microsoft.Win32.Primitives.dll", Microsoft_Win32_Primitives_dll_32937B25, Microsoft_Win32_Primitives_dll_32937B25_len);
  mono_wasm_add_assembly ("System.Collections.NonGeneric.dll", System_Collections_NonGeneric_dll_828334FC, System_Collections_NonGeneric_dll_828334FC_len);
  mono_wasm_add_assembly ("System.Threading.dll", System_Threading_dll_36888B71, System_Threading_dll_36888B71_len);
  mono_wasm_add_assembly ("System.Runtime.dll", System_Runtime_dll_19ED86F4, System_Runtime_dll_19ED86F4_len);
  mono_wasm_add_assembly ("System.Private.Uri.dll", System_Private_Uri_dll_A6A36638, System_Private_Uri_dll_A6A36638_len);
  mono_wasm_add_assembly ("System.Diagnostics.Tracing.dll", System_Diagnostics_Tracing_dll_08CB5321, System_Diagnostics_Tracing_dll_08CB5321_len);
  mono_wasm_add_assembly ("System.Memory.dll", System_Memory_dll_5E9BA1A6, System_Memory_dll_5E9BA1A6_len);
  mono_wasm_add_assembly ("System.Runtime.InteropServices.dll", System_Runtime_InteropServices_dll_C9CC0A41, System_Runtime_InteropServices_dll_C9CC0A41_len);
  mono_wasm_add_assembly ("Fermyon.Spin.Sdk.dll", Fermyon_Spin_Sdk_dll_A4A336FA, Fermyon_Spin_Sdk_dll_A4A336FA_len);
  mono_wasm_add_assembly ("System.Net.Http.dll", System_Net_Http_dll_BD4445D7, System_Net_Http_dll_BD4445D7_len);
  mono_wasm_add_assembly ("System.Private.Runtime.InteropServices.JavaScript.dll", System_Private_Runtime_InteropServices_JavaScript_dll_F1F71C0B, System_Private_Runtime_InteropServices_JavaScript_dll_F1F71C0B_len);
  mono_wasm_add_assembly ("System.Net.Security.dll", System_Net_Security_dll_BF6427BB, System_Net_Security_dll_BF6427BB_len);
  mono_wasm_add_assembly ("System.Security.Cryptography.dll", System_Security_Cryptography_dll_79859BDF, System_Security_Cryptography_dll_79859BDF_len);
  mono_wasm_add_assembly ("System.Text.Encoding.Extensions.dll", System_Text_Encoding_Extensions_dll_E5113A13, System_Text_Encoding_Extensions_dll_E5113A13_len);
  mono_wasm_add_assembly ("System.Collections.Concurrent.dll", System_Collections_Concurrent_dll_60ED355F, System_Collections_Concurrent_dll_60ED355F_len);
  mono_wasm_add_assembly ("System.Runtime.Numerics.dll", System_Runtime_Numerics_dll_98BA933F, System_Runtime_Numerics_dll_98BA933F_len);
  mono_wasm_add_assembly ("System.Formats.Asn1.dll", System_Formats_Asn1_dll_3A51E30A, System_Formats_Asn1_dll_3A51E30A_len);
  mono_wasm_add_assembly ("System.Diagnostics.DiagnosticSource.dll", System_Diagnostics_DiagnosticSource_dll_DDB8C4E1, System_Diagnostics_DiagnosticSource_dll_DDB8C4E1_len);
  mono_wasm_add_assembly ("System.Threading.Thread.dll", System_Threading_Thread_dll_477DC647, System_Threading_Thread_dll_477DC647_len);
  mono_wasm_add_assembly ("System.Collections.Immutable.dll", System_Collections_Immutable_dll_8C4396FF, System_Collections_Immutable_dll_8C4396FF_len);
  mono_wasm_add_assembly ("System.Linq.dll", System_Linq_dll_00E030BE, System_Linq_dll_00E030BE_len);
  mono_wasm_add_assembly ("System.Numerics.Vectors.dll", System_Numerics_Vectors_dll_941BE9F4, System_Numerics_Vectors_dll_941BE9F4_len);
}

