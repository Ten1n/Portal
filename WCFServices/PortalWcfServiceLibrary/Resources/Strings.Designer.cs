﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConfirmIt.Portal.WcfServiceLibrary.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ConfirmIt.Portal.WcfServiceLibrary.Resources.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to Авторизация не выполнена..
        /// </summary>
        internal static string AuthenticationFail {
            get {
                return ResourceManager.GetString("AuthenticationFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка при получении событий пользователя {0} за {1}..
        /// </summary>
        internal static string GerUserEventsError {
            get {
                return ResourceManager.GetString("GerUserEventsError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка при получении информации о дне..
        /// </summary>
        internal static string GetCalendarItemError {
            get {
                return ResourceManager.GetString("GetCalendarItemError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка при получении имени офиса..
        /// </summary>
        internal static string GetOfficeNameError {
            get {
                return ResourceManager.GetString("GetOfficeNameError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка при получении списка пользователей..
        /// </summary>
        internal static string GetUsersListError {
            get {
                return ResourceManager.GetString("GetUsersListError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка при возвращении статусов пользователей..
        /// </summary>
        internal static string GetUsersStatusesError {
            get {
                return ResourceManager.GetString("GetUsersStatusesError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сервис PortalWeb для офиса {0} запущен..
        /// </summary>
        internal static string ServiceStarted {
            get {
                return ResourceManager.GetString("ServiceStarted", resourceCulture);
            }
        }
    }
}