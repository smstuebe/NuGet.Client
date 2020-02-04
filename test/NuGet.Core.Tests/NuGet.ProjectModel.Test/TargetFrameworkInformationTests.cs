// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class TargetFrameworkInformationTests
    {

        [Fact]
        public void Equals_WithSameObject_ReturnsTrue()
        {
            // Arrange
            var tfi = CreateTargetFrameworkInformation();

            // Act & Assert
            tfi.Equals(tfi).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithSameContent_ReturnsTrue()
        {
            // Arrange
            var tfi = CreateTargetFrameworkInformation();
            var tfiTwin = CreateTargetFrameworkInformation();

            // Act & Assert
            tfi.Equals(tfiTwin).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithDifferentObject_CentralDependency_ReturnsFalse()
        {
            // Arrange
            var tfiFoo = CreateTargetFrameworkInformation(new List<CentralVersionDependency>() { new CentralVersionDependency("foo", VersionRange.All) });
            var tfiBar = CreateTargetFrameworkInformation(new List<CentralVersionDependency>() { new CentralVersionDependency("bar", VersionRange.All) });

            // Act & Assert
            tfiFoo.Equals(tfiBar).Should().BeFalse();
        }

        [Fact]
        public void Equals_OnClone_ReturnsTrue()
        {
            // Arrange
            var tfi = CreateTargetFrameworkInformation();
            var tfiClone = tfi.Clone();

            // Act & Assert
            tfi.Equals(tfiClone).Should().BeTrue();
        }

        private TargetFrameworkInformation CreateTargetFrameworkInformation()
        {
            return CreateTargetFrameworkInformation(new List<CentralVersionDependency>(){ new CentralVersionDependency("foo", VersionRange.All), new CentralVersionDependency("bar", VersionRange.AllStable) });
        }

        private TargetFrameworkInformation CreateTargetFrameworkInformation(List<CentralVersionDependency> centralVersionDependencies)
        {
            NuGetFramework nugetFramework = new NuGetFramework("net40");
            var dependencyFoo = new LibraryDependency(new LibraryRange("foo", VersionRange.All, LibraryDependencyTarget.All),
            LibraryDependencyType.Default,
            LibraryIncludeFlags.All,
            LibraryIncludeFlags.All, new List<Common.NuGetLogCode>(), true, true);

            var downloadDependency = new DownloadDependency("foo", VersionRange.All);
            var frameworkDependency = new FrameworkDependency("framework", FrameworkDependencyFlags.All);

            var dependencies = new List<LibraryDependency>() { dependencyFoo };
            var assetTargetFallback = true;
            var warn = false;

            TargetFrameworkInformation tfi = new TargetFrameworkInformation()
            {
                AssetTargetFallback = assetTargetFallback,
                Dependencies = dependencies,
                Warn = warn,
                FrameworkName = nugetFramework,
            };

            foreach (var cdep in centralVersionDependencies)
            {
                tfi.CentralVersionDependencies.Add(cdep.Name, cdep);
            }

            tfi.DownloadDependencies.Add(downloadDependency);
            tfi.FrameworkReferences.Add(frameworkDependency);

            return tfi;
        }
    }
}