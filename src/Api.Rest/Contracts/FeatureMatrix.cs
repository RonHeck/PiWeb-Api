﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	#region usings

	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	public abstract class FeatureMatrix
	{
		#region constants

		protected const int SupportedMajorVersion = 1;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureMatrix"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceVersionRange"/> is <see langword="null" />.</exception>
		protected FeatureMatrix( [NotNull] InterfaceVersionRange interfaceVersionRange )
		{
			if( interfaceVersionRange == null ) throw new ArgumentNullException( nameof( interfaceVersionRange ) );

			CurrentInterfaceVersion = GetBestKnownVersion( interfaceVersionRange.SupportedVersions );
		}

		#endregion

		#region properties

		public Version CurrentInterfaceVersion { get; }

		#endregion

		#region methods

		protected static Version GetBestKnownVersion( [NotNull] IEnumerable<Version> supportedVersions )
		{
			if( supportedVersions == null ) throw new ArgumentNullException( nameof( supportedVersions ) );

			Version bestKnownVersion = null;
			foreach( var versionCanditate in supportedVersions )
			{
				var isKnownVersion = versionCanditate >= new Version( SupportedMajorVersion, 0 ) && versionCanditate < new Version( SupportedMajorVersion + 1, 0 );
				var isSuperiorToBestKnownVersion = bestKnownVersion == null || versionCanditate > bestKnownVersion;

				if( isKnownVersion && isSuperiorToBestKnownVersion )
					bestKnownVersion = versionCanditate;
			}

			if( bestKnownVersion == null )
				throw new ServerApiNotSupportedException();

			return bestKnownVersion;
		}

		#endregion
	}
}