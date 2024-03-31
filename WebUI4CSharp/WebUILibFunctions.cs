﻿using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace WebUI4CSharp
{
    /// <summary>
    /// Class with all the functions exported by the WebUI library.
    /// </summary>
    public static class WebUILibFunctions
    {
#if WEBUIDEMO
        // The demos define the WEBUIDEMO conditional for convenience
        // Enable the right library depending on the selected platform target in each demo
        private const string _LibName = "..\\..\\..\\..\\WebUI_binaries\\64bits\\webui-2.dll";  // Any CPU
                                                                                                    //private const string _LibName = "..\\..\\..\\..\\..\\..\\WebUI_binaries\\64bits\\webui-2.dll"; // x64 
                                                                                                    //private const string _LibName = "..\\..\\..\\..\\..\\..\\WebUI_binaries\\64bits\\webui-2_debug.dll"; // x64 (verbose logging) 
                                                                                                    //private const string _LibName = "..\\..\\..\\..\\..\\..\\WebUI_binaries\\32bits\\webui-2.dll"; // x32 
                                                                                                    //private const string _LibName = "..\\..\\..\\..\\..\\..\\WebUI_binaries\\32bits\\webui-2_debug.dll"; // x32 (verbose logging)
#else
        private const string _LibName = "WebUI_binaries\\64bits\\webui-2.dll";    
#endif

        /// <summary>
        /// Create a new WebUI window object.
        /// </summary>
        /// <returns>Returns the window number.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_new_window)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention=CallingConvention.Cdecl)]
        public static extern UIntPtr webui_new_window();

        /// <summary>
        /// Create a new webui window object using a specified window number.
        /// </summary>
        /// <param name="window_number">The window number (should be > 0, and < WEBUI_MAX_IDS).</param>
        /// <returns>Returns the window number.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_new_window_id)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr webui_new_window_id(UIntPtr window_number);

        /// <summary>
        /// Get a free window number that can be used with `webui_new_window_id()`.
        /// </summary>
        /// <returns>Returns the first available free window number. Starting from 1.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_new_window_id)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr webui_get_new_window_id();

        /// <summary>
        /// Bind a specific html element click event with a function. Empty element means all events.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="element">The HTML ID.</param>
        /// <param name="func">The callback function.</param>
        /// <returns>Returns a unique bind ID.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_bind)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr webui_bind(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string element, BindCallback func);

        /// <summary>
        /// Show a window using embedded HTML, or a file. If the window is already open, it will be refreshed.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="content">The HTML, URL, Or a local file.</param>
        /// <returns>Returns True if showing the window is successed.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_show)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_show(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string content);

        /// <summary>
        /// Same as `webui_show()`. But using a specific web browser.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="content">The HTML, URL, Or a local file.</param>
        /// <param name="browser">The web browser to be used.</param>
        /// <returns>Returns True if showing the window is successed.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_show_browser)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_show_browser(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string content, UIntPtr browser);

        /// <summary>
        /// Set the window in Kiosk mode (Full screen).
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="status">True or False.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_kiosk)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_kiosk(UIntPtr window, [MarshalAs(UnmanagedType.I1)] bool status);

        /// <summary>
        /// Wait until all opened windows get closed.
        /// </summary>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_wait)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_wait();

        /// <summary>
        /// Close a specific window only. The window object will still exist.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_close)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_close(UIntPtr window);

        /// <summary>
        /// Close a specific window and free all memory resources.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_destroy)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_destroy(UIntPtr window);

        /// <summary>
        /// Close all open windows. `webui_wait()` will return (Break).
        /// </summary>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_exit)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_exit();

        /// <summary>
        /// Set the web-server root folder path for a specific window.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="path">The local folder full path.</param>
        /// <returns>Returns True if the function was successful.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_root_folder)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_set_root_folder(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);

        /// <summary>
        /// Set the web-server root folder path for all windows. Should be used before `webui_show()`.
        /// </summary>
        /// <param name="path">The local folder full path.</param>
        /// <returns>Returns True if the function was successful.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_default_root_folder)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_set_default_root_folder([MarshalAs(UnmanagedType.LPUTF8Str)] string path);

        /// <summary>
        /// Set a custom handler to serve files.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="handler">The handler function: `void myHandler(const char* filename, * int* length)`.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_file_handler)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_file_handler(UIntPtr window, FileHandlerCallback handler);

        /// <summary>
        /// Check if the specified window is still running.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <returns>Returns True if the window is still running.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_is_shown)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_is_shown(UIntPtr window);

        /// <summary>
        /// Set the maximum time in seconds to wait for the browser to start.
        /// </summary>
        /// <param name="second">The timeout in seconds.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_timeout)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_timeout(UIntPtr second);

        /// <summary>
        /// Set the default embedded HTML favicon.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="icon">The icon as string: `<svg>...</svg>`.</param>
        /// <param name="icon_type">The icon type: `image/svg+xml`.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_icon)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_icon(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string icon, [MarshalAs(UnmanagedType.LPUTF8Str)] string icon_type);

        /// <summary>
        /// Base64 encoding. Use this to safely send text based data to the UI. If it fails it will return NULL.
        /// </summary>
        /// <param name="str">The string to encode (Should be null terminated).</param>
        /// <returns>Returns a encoded string.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_encode)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPUTF8Str)]
        public static extern string webui_encode([MarshalAs(UnmanagedType.LPUTF8Str)] string str);

        /// <summary>
        /// Base64 decoding. Use this to safely decode received Base64 text from the UI. If it fails it will return NULL.
        /// </summary>
        /// <param name="str">The string to decode (Should be null terminated).</param>
        /// <returns>Returns a decoded string.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_decode)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPUTF8Str)]
        public static extern string webui_decode([MarshalAs(UnmanagedType.LPUTF8Str)] string str);

        /// <summary>
        /// Safely free a buffer allocated by WebUI using `webui_malloc()`.
        /// </summary>
        /// <param name="ptr">The buffer to be freed.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_free)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_free(IntPtr ptr);

        /// <summary>
        /// Safely allocate memory using the WebUI memory management system. It can be safely freed using `webui_free()` at any time.
        /// </summary>
        /// <param name="size">The size of memory in bytes.</param>
        /// <returns>Returns a pointer to the allocated memory.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_malloc)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr webui_malloc(UIntPtr size);

        /// <summary>
        /// Safely send raw data to the UI.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="function">The JavaScript function to receive raw data: `function * myFunc(myData){}`.</param>
        /// <param name="raw">The raw data buffer.</param>
        /// <param name="size">The raw data size in bytes.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_send_raw)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_send_raw(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string function, UIntPtr raw, UIntPtr size);

        /// <summary>
        /// Set a window in hidden mode. Should be called before `webui_show()`.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="status">The status: True or False.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_hide)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_hide(UIntPtr window, [MarshalAs(UnmanagedType.I1)] bool status);

        /// <summary>
        /// Set the window size.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="width">The window width.</param>
        /// <param name="height">The window height.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_size)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_size(UIntPtr window, uint width, uint height);

        /// <summary>
        /// Set the window position.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="x">The window X.</param>
        /// <param name="y">The window Y.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_position)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_position(UIntPtr window, uint x, uint y);

        /// <summary>
        /// Set the web browser profile to use. An empty `name` and `path` means the default user profile. Need to be called before `webui_show()`.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="name">The web browser profile name.</param>
        /// <param name="path">The web browser profile full path.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_profile)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_profile(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);

        /// <summary>
        /// Set the web browser proxy_server to use. Need to be called before 'webui_show()'.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="proxy_server">The web browser proxy_server. For example 'http://127.0.0.1:8888'</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_proxy)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_proxy(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string proxy_server);

        /// <summary>
        /// Get the full current URL.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <returns>Returns the full URL string.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_url)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPUTF8Str)]
        public static extern string webui_get_url(UIntPtr window);

        /// <summary>
        /// Allow a specific window address to be accessible from a public network.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="status">True or False.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_public)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_public(UIntPtr window, [MarshalAs(UnmanagedType.I1)] bool status);

        /// <summary>
        /// Navigate to a specific URL.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="url">Full HTTP URL.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_navigate)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_navigate(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);

        /// <summary>
        /// Free all memory resources. Should be called only at the end.
        /// </summary>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_clean)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_clean();

        /// <summary>
        /// Delete all local web-browser profiles folder. It should called at the end.
        /// </summary>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_delete_all_profiles)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_delete_all_profiles();

        /// <summary>
        /// Delete a specific window web-browser local folder profile.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <remarks>
        /// <para>This can break functionality of other windows if using the same web-browser.</para>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_delete_profile)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_delete_profile(UIntPtr window);

        /// <summary>
        /// Get the ID of the parent process (The web browser may re-create another new process).
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <returns>Returns the the parent process id as integer.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_parent_process_id)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr webui_get_parent_process_id(UIntPtr window);

        /// <summary>
        /// Get the ID of the last child process.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <returns>Returns the the child process id as integer.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_child_process_id)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr webui_get_child_process_id(UIntPtr window);

        /// <summary>
        /// Set a custom web-server network port to be used by WebUI.
        /// This can be useful to determine the HTTP link of `webui.js` in case
        /// you are trying to use WebUI with an external web-server like NGNIX
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="port">The web-server network port WebUI should use.</param>
        /// <returns>Returns True if the port is free and usable by WebUI.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_port)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_set_port(UIntPtr window, UIntPtr port);

        /// <summary>
        /// Set the SSL/TLS certificate and the private key content, both in PEM
        /// format. This works only with `webui-2-secure` library. If set empty WebUI
        /// will generate a self-signed certificate.
        /// </summary>
        /// <param name="certificate_pem">The SSL/TLS certificate content in PEM format.</param>
        /// <param name="private_key_pem">The private key content in PEM format.</param>
        /// <returns>Returns True if the certificate and the key are valid.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_tls_certificate)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_set_tls_certificate([MarshalAs(UnmanagedType.LPUTF8Str)] string certificate_pem, [MarshalAs(UnmanagedType.LPUTF8Str)] string private_key_pem);

        /// <summary>
        /// Run JavaScript without waiting for the response.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="script">The JavaScript to be run.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_run)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_run(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string script);

        /// <summary>
        /// Run JavaScript and get the response back.
        /// Make sure your local buffer can hold the response.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="script">The JavaScript to be run.</param>
        /// <param name="timeout">The execution timeout.</param>
        /// <param name="buffer">The local buffer to hold the response.</param>
        /// <param name="buffer_length">The local buffer size.</param>
        /// <returns>Returns True if there is no execution error.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_script)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_script(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string script, UIntPtr timeout, IntPtr buffer, UIntPtr buffer_length);

        /// <summary>
        /// Chose between Deno and Nodejs as runtime for .js and .ts files.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="runtime">Deno or Nodejs.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_set_runtime)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_set_runtime(UIntPtr window, UIntPtr runtime);

        /// <summary>
        /// Get an argument as integer at a specific index.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <param name="index">The argument position starting from 0.</param>
        /// <returns>Returns argument as integer.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_int_at)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long webui_get_int_at(ref webui_event_t e, UIntPtr index);

        /// <summary>
        /// Get the first argument as integer.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <returns>Returns argument as integer.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_int)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long webui_get_int(ref webui_event_t e);

        /// <summary>
        /// Get an argument as string at a specific index.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <param name="index">The argument position starting from 0.</param>
        /// <returns>Returns argument as string.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_string_at)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr webui_get_string_at(ref webui_event_t e, UIntPtr index);

        /// <summary>
        /// Get the first argument as string.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <returns>Returns argument as string.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_string)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr webui_get_string(ref webui_event_t e);

        /// <summary>
        /// Get an argument as boolean at a specific index.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <param name="index">The argument position starting from 0.</param>
        /// <returns>Returns argument as boolean.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_bool_at)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_get_bool_at(ref webui_event_t e, UIntPtr index);

        /// <summary>
        /// Get the first argument as boolean.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <returns>Returns argument as boolean.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_bool)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_get_bool(ref webui_event_t e);

        /// <summary>
        /// Get the size in bytes of an argument at a specific index.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <param name="index">The argument position starting from 0.</param>
        /// <returns>Returns size in bytes.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_size_at)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr webui_get_size_at(ref webui_event_t e, UIntPtr index);

        /// <summary>
        /// Get size in bytes of the first argument.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <returns>Returns size in bytes.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_get_size)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr webui_get_size(ref webui_event_t e);

        /// <summary>
        /// Return the response to JavaScript as integer.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <param name="n">The integer to be send to JavaScript.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_return_int)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_return_int(ref webui_event_t e, long n);

        /// <summary>
        /// Return the response to JavaScript as string.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <param name="s">The string to be send to JavaScript.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_return_string)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_return_string(ref webui_event_t e, [MarshalAs(UnmanagedType.LPUTF8Str)] string s);

        /// <summary>
        /// Return the response to JavaScript as a buffer.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <param name="buffer">The buffer to be send to JavaScript.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_return_string)</see></para>
        /// </remarks>
        [DllImport("webui-2", EntryPoint = "webui_return_string")]
        public static extern void webui_return_buffer(ref webui_event_t e, ref byte[] buffer);

        /// <summary>
        /// Return the response to JavaScript as boolean.
        /// </summary>
        /// <param name="e">The event struct.</param>
        /// <param name="b">The boolean to be send to JavaScript.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_return_bool)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_return_bool(ref webui_event_t e, [MarshalAs(UnmanagedType.I1)] bool b);

        /// <summary>
        /// Bind a specific HTML element click event with a function. Empty element means all events.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="element">The element ID.</param>
        /// <param name="func">The callback as myFunc(Window, EventType, Element, EventNumber, BindID).</param>
        /// <returns>Returns unique bind ID.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_interface_bind)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr webui_interface_bind(UIntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string element, InterfaceEventCallback func);

        /// <summary>
        /// When using `webui_interface_bind()`, you may need this function to easily set a response.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="event_number">The event number.</param>
        /// <param name="response">The response as string to be send to JavaScript.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_interface_set_response)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void webui_interface_set_response(UIntPtr window, UIntPtr event_number, [MarshalAs(UnmanagedType.LPUTF8Str)] string response);

        /// <summary>
        /// When using `webui_interface_bind()`, you may need this function to easily set a response.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="event_number">The event number.</param>
        /// <param name="buffer">The response as a buffer to be send to JavaScript.</param>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_interface_set_response)</see></para>
        /// </remarks>
        [DllImport("webui-2", EntryPoint = "webui_interface_set_response")]
        public static extern void webui_interface_set_buffer_response(UIntPtr window, UIntPtr event_number, ref byte[] buffer);

        /// <summary>
        /// Check if the app still running.
        /// </summary>
        /// <returns>Returns True if app is running.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_interface_is_app_running)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_interface_is_app_running();

        /// <summary>
        /// Get a unique window ID.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <returns>Returns the unique window ID as integer.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_interface_get_window_id)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr webui_interface_get_window_id(UIntPtr window);

        /// <summary>
        /// Get an argument as string at a specific index.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="event_number">The event number.</param>
        /// <param name="index">The argument position.</param>
        /// <returns>Returns argument as string.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_interface_get_string_at)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr webui_interface_get_string_at(UIntPtr window, UIntPtr event_number, UIntPtr index);

        /// <summary>
        /// Get an argument as integer at a specific index.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="event_number">The event number.</param>
        /// <param name="index">The argument position.</param>
        /// <returns>Returns argument as integer.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_interface_get_int_at)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long webui_interface_get_int_at(UIntPtr window, UIntPtr event_number, UIntPtr index);

        /// <summary>
        /// Get an argument as boolean at a specific index.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="event_number">The event number.</param>
        /// <param name="index">The argument position.</param>
        /// <returns>Returns argument as boolean.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_interface_get_bool_at)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool webui_interface_get_bool_at(UIntPtr window, UIntPtr event_number, UIntPtr index);

        /// <summary>
        /// Get the size in bytes of an argument at a specific index.
        /// </summary>
        /// <param name="window">The window number.</param>
        /// <param name="event_number">The event number.</param>
        /// <param name="index">The argument position.</param>
        /// <returns>Returns size in bytes.</returns>
        /// <remarks>
        /// <para><see href="https://github.com/webui-dev/webui/blob/main/include/webui.h">WebUI source file: /include/webui.h (webui_interface_get_size_at)</see></para>
        /// </remarks>
        [DllImport(_LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr webui_interface_get_size_at(UIntPtr window, UIntPtr event_number, UIntPtr index);
    }
}
