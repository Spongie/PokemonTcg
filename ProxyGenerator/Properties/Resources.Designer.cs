﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProxyGenerator.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ProxyGenerator.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to public void {METHODNAME}({PARAMS})
        ///	{
        ///		var message = new GenericMessageData
        ///		{
        ///			TargetMethod = &quot;{METHODNAME}&quot;,
        ///			TargetClass = &quot;{SERVICENAME}&quot;,
        ///			Parameters = new object[] { {PARAMVALUES} }
        ///		}.ToNetworkMessage(networkPlayer.Id);
        ///		
        ///		return networkPlayer.Send(message);
        ///	}.
        /// </summary>
        internal static string AsyncMethodTemplateV1 {
            get {
                return ResourceManager.GetString("AsyncMethodTemplateV1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using NetworkingCore;
        ///using NetworkingCore.Messages;
        ///
        ////// &lt;summary&gt;
        ////// Auto-generated code - DO NOT EDIT
        ////// &lt;/summary&gt;
        ///public class Async{SERVICENAME}
        ///{
        ///	private readonly NetworkPlayer networkPlayer;
        ///	
        ///	public Async{SERVICENAME}(NetworkPlayer networkPlayer)
        ///	{
        ///		this.networkPlayer = networkPlayer;
        ///	}
        ///	
        ///	{METHODS}
        ///}.
        /// </summary>
        internal static string AsyncServiceTemplateV1 {
            get {
                return ResourceManager.GetString("AsyncServiceTemplateV1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to public {RETURNTYPE} {METHODNAME}({PARAMS})
        ///	{
        ///		var message = new GenericMessageData
        ///		{
        ///			TargetMethod = &quot;{METHODNAME}&quot;,
        ///			TargetClass = &quot;{SERVICENAME}&quot;,
        ///			Parameters = new object[] { {PARAMVALUES} }
        ///		}.ToNetworkMessage(networkPlayer.Id);
        ///		
        ///		return networkPlayer.SendAndWaitForResponse&lt;{RETURNTYPE}&gt;(message);
        ///	}.
        /// </summary>
        internal static string MethodTemplateV1 {
            get {
                return ResourceManager.GetString("MethodTemplateV1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using NetworkingCore;
        ///using NetworkingCore.Messages;
        ///
        ////// &lt;summary&gt;
        ////// Auto-generated code - DO NOT EDIT
        ////// &lt;/summary&gt;
        ///public class {SERVICENAME}
        ///{
        ///	private readonly NetworkPlayer networkPlayer;
        ///	
        ///	public {SERVICENAME}(NetworkPlayer networkPlayer)
        ///	{
        ///		this.networkPlayer = networkPlayer;
        ///	}
        ///	
        ///	{METHODS}
        ///}.
        /// </summary>
        internal static string ServiceTemplateV1 {
            get {
                return ResourceManager.GetString("ServiceTemplateV1", resourceCulture);
            }
        }
    }
}
