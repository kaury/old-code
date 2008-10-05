//
// Microsfot.Web.Services.Routing.Via.cs
//
// Author: Daniel Kornhauser <dkor@alum.mit.edu>
//
// Copyright (C) Ximian, Inc. 2003
//

using System;

namespace Microsoft.Web.Services.Routing {

	public class Via: ICloneable
	{
		Uri val;
		Uri vid;

		public Via()
		{
		}

		public Via (Uri value)
		{
			this.val = value;
		}

		private Via (Uri value, Uri vid)
		{
			this.val = value;
			this.vid = vid;
		}

		public Uri Value
		{
			get { return val; }
			set { val = value; }
		}

		public Uri Vid 
		{
			get { return vid; }
			set { vid = value; }
		}

#if WSE1
		public object Clone ()
#else
		public virtual object Clone ()
#endif
		{
			return new Via (val, vid);
		}
		
	}
}		

		