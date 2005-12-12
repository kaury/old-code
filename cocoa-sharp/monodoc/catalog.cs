/// Catalog
// Copyright (C) 2004 Edd Dumbill <edd@usefulinc.com>
///
/// This program is free software; you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation; either version 2 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

/// catelog.cs I18N binding for gettext

using System;
using System.Runtime.InteropServices;



class Catalog {
	[DllImport("libc")]
	static extern IntPtr bindtextdomain (IntPtr domainname, IntPtr dirname);
	[DllImport("libc")]
	static extern IntPtr bind_textdomain_codeset (IntPtr domainname,
		IntPtr codeset);
	[DllImport("libc")]
	static extern IntPtr textdomain (IntPtr domainname);
	
	public static void Init (String package, String localedir)
	{
		IntPtr ipackage = Marshal.StringToHGlobalAuto (package);
		IntPtr ilocaledir = Marshal.StringToHGlobalAuto (localedir);
		IntPtr iutf8 = Marshal.StringToHGlobalAuto ("UTF-8");
		bindtextdomain (ipackage, ilocaledir);
		bind_textdomain_codeset (ipackage, iutf8);
		textdomain (ipackage);
		Marshal.FreeHGlobal (ipackage);
		Marshal.FreeHGlobal (ilocaledir);
		Marshal.FreeHGlobal (iutf8);
	}

	[DllImport("libc")]
	static extern IntPtr gettext (IntPtr instring);
	
	public static String GetString (String s)
	{
		IntPtr ints = Marshal.StringToHGlobalAuto (s);
		String t = Marshal.PtrToStringAuto (gettext (ints));
		Marshal.FreeHGlobal (ints);
		return t;
	}

	[DllImport("libc")]
	static extern IntPtr ngettext (IntPtr singular, IntPtr plural, Int32 n);
	
	public static String GetPluralString (String s, String p, Int32 n)
	{
		IntPtr ints = Marshal.StringToHGlobalAuto (s);
		IntPtr intp = Marshal.StringToHGlobalAuto (p);
		String t = Marshal.PtrToStringAnsi (ngettext (ints, intp, n));
		Marshal.FreeHGlobal (ints);
		Marshal.FreeHGlobal (intp);
		return t;
	}

	public static void InitCatalog ()
	{
		Catalog.Init ("monodoc", "/usr/local/share/locale");
	}
}