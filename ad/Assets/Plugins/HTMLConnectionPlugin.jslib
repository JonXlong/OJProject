var HTMLConnectionPlugin = {
	RequestConnection: function() {
		return getSocketURL();
	}
};

mergeInto(LibraryManager.library, HTMLConnectionPlugin);