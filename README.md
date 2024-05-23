# ReadonlyBytes


Sometimes it is necessary to compare arrays by their values, not by reference (the default comparison method).
ReadonlyBytes class is an example of a class-wrapper, that adds the above mentioned functionality by overriding an Equals() method.
Also, it has IEnumerator; overridened ToString() and GetHashCode() methods.
GetHasCode() calculates the hash of bytes array by using FNV algorithm.  