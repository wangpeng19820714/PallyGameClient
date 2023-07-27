#target photoshop
app.bringToFront();

var scriptVersion = 1.2;

var cs2 = parseInt(app.version) < 10;

var defaultSettings = {
	writeTemplate: false,
	writeJson: true,
	scale: 1,
	padding: 1,
	psdDir: "D:\\PallyGame\\Assets\\Res\\UITextures"
};
var settings = loadSettings();
showDialog();

var progress, cancel, errors;
function run () {
    var jsonFile = new File(jsonPath(settings.jsonPath));
	jsonFile.parent.create();
	var psdDir = absolutePath(settings.psdDir);
}

function showDialog () {

    /*
	try {
		decodeURI(activeDocument.path);
	} catch (e) {
		alert("请先打开任意的PSD文件，再使用CyberTools.");
		return;
	}
    */

    var dialog = new Window("dialog", "PallyGameUI导出工具 v" + scriptVersion), group;
    dialog.alignChildren = "fill";

    var settingsGroup = dialog.add("panel", undefined, "Settings");
    settingsGroup.margins = [10,15,10,10];
    settingsGroup.alignChildren = "fill";

	var outputPathGroup = dialog.add("panel", undefined, "资源路径配置");
		outputPathGroup.alignChildren = ["fill", ""];
		outputPathGroup.margins = [10,15,10,10];
		var psdDirText, psdDirPreview;
		if (!cs2) {
			var textGroup = outputPathGroup.add("group");
			textGroup.orientation = "column";
			textGroup.alignChildren = ["fill", ""];
			group = textGroup.add("group");
				group.add("statictext", undefined, "PSDs:");
				psdDirText = group.add("edittext", undefined, settings.psdDir);
				psdDirText.alignment = ["fill", ""];
			psdDirPreview = textGroup.add("statictext", undefined, "");
			psdDirPreview.maximumSize.width = 260;
		} else {
			outputPathGroup.add("statictext", undefined, "PSDs:");
			psdDirText = outputPathGroup.add("edittext", undefined, settings.psdDir);
			psdDirText.alignment = "fill";
		}

    var functionGroup = dialog.add("panel", undefined, "功能区");
        functionGroup.margins = [10,15,10,10];
        functionGroup.alignChildren = "fill";

        var buttonGroup = functionGroup.add("group");
            var createButton = buttonGroup.add("button", undefined, "Create New UI PSD");
            var outputButton = buttonGroup.add("button", undefined, "Output Active UI Res");
            buttonGroup.orientation = "column";
            buttonGroup.alignChildren = ["fill", ""];
            group = buttonGroup.add("group");

    var svnGroup = dialog.add("panel", undefined, "SVN 功能区");
        svnGroup.margins = [10,15,10,10];
        svnGroup.alignChildren = "fill";

        var svnButtonGroup = svnGroup.add("group");
            var updatePSDButton = svnButtonGroup.add("button", undefined, "Update All PSD From SVN");
            var commitPSDButton = svnButtonGroup.add("button", undefined, "Commit All UI Res To SVN");
            svnButtonGroup.orientation = "column";
            svnButtonGroup.alignChildren = ["fill", ""];
            group = svnButtonGroup.add("group");

    var cancelButtonGroup = dialog.add("group");
        group = cancelButtonGroup.add("group");
        group.alignment = ["fill", ""];
        group.alignChildren = ["right", ""];
        var saveButton = group.add("button", undefined, "Save");
        var cancelButton = group.add("button", undefined, "Cancel");

	psdDirText.onChanging = function () {
        /*
		var text = psdDirText.text ? absolutePath(psdDirText.text) : "<no Psds output>";
		if (!cs2) {
			psdDirPreview.text = text;
        }*/
	};

    psdDirText.onChanging();

	function updateSettings () {
		settings.writeTemplate = writeTemplateCheckbox.value;
		settings.writeJson = writeJsonCheckbox.value;
		settings.trimWhitespace = trimWhitespaceCheckbox.value;

		var scaleValue = parseFloat(scaleText.text);
		if (scaleValue > 0 && scaleValue <= 100) settings.scale = scaleValue / 100;

		settings.psdDirText = psdDirText.text;

		var paddingValue = parseInt(paddingText.text);
		if (paddingValue >= 0) settings.padding = paddingValue;
	}

    // 创建新的UI PSD文件
    createButton.onClick = function () {
        CreateNewPSD(dialog);
    }

    // 输出当前激活PSD文件
    outputButton.onClick = function () {
        OutputActive(dialog)
    }

    // 从SVN更新PSD文件
    updatePSDButton.onClick = function () {
    }

    // 提交所有UI资源文件到SVN
    commitPSDButton.onClick = function () {
    }

    // 保存当前配置
    saveButton.onClick = function () {
        savePath(psdDirText.text);
    }

    dialog.center();
	dialog.show();
}

function loadSettings () {
	var options = null;
	try {
		options = app.getCustomOptions(sID("settings"));
	} catch (e) {
	}

	var settings = {};
	for (var key in defaultSettings) {
		if (!defaultSettings.hasOwnProperty(key)) continue;
		var typeID = sID(key);
		if (options && options.hasKey(typeID))
			settings[key] = options["get" + getOptionType(defaultSettings[key])](typeID);
		else
			settings[key] = defaultSettings[key];
	}
	return settings;
}

function saveSettings () {
	if (cs2) return; // No putCustomOptions.
	var action = new ActionDescriptor();
	for (var key in defaultSettings) {
		if (!defaultSettings.hasOwnProperty(key)) continue;
		action["put" + getOptionType(defaultSettings[key])](sID(key), settings[key]);
	}
	app.putCustomOptions(sID("settings"), action, true);
}

function scriptDir () {
	var file;
	if (!cs2)
		file = $.fileName;
	else {
		try {
			var error = THROW_ERROR; // Force error which provides the script file name.
		} catch (ex) {
			file = ex.fileName;
		}
	}
	return new File(file).parent + "/";
}

function absolutePath (path) {
	path = trim(path);
	if (!startsWith(path, "./")) {
		var absolute = decodeURI(new File(path).absoluteURI);
		if (!startsWith(absolute, decodeURI(new File("child").parent.absoluteURI))) return absolute + "/";
		path = "./" + path;
	}

	if (path.length == 0)
		path = decodeURI(activeDocument.path);
	else if (startsWith(settings.psdDir, "./"))
		path = decodeURI(activeDocument.path) + path.substring(1);
	path = (new File(path).fsName).toString();
	path = path.replace(/\\/g, "/");
	if (path.substring(path.length - 1) != "/") path += "/";
	return path;
}

function cID (id) {
	return charIDToTypeID(id);
}

function sID (id) {
	return stringIDToTypeID(id);
}

function CreateNewPSD(dialog){
    app.documents.add(1920, 1080, 96, "NewUI", NewDocumentMode.RGB)
    var outputSets = app.activeDocument.layerSets.add()
    outputSets.name = "Output Folder"

    var newUISets = app.activeDocument.layerSets.add()
    newUISets.name = "Desgin Folder"

    var importResSets = app.activeDocument.layerSets.add()
    importResSets.name = "Require Folder"
    dialog.close();
}

function OpenCommonTempalte(dialog){
    var templatePath = settings.psdDir;
    if (templatePath == undefined)
    {
        alert("无法找到模版文件所在路径！")
    }
    else
    {
        templatePath = templatePath + "/PSDs/Template.psd"
        var doc = app.open( new File(templatePath))
        dialog.close();
    }
}

function OutputActive(dialog){
    var outputPath = settings.psdDir;
    if (outputPath == undefined)
    {
        alert("无法找到模版文件所在路径！")
    }
    else
    {
        var doc = app.activeDocument;
        var selBorad = app.activeDocument.activeLayer;
        var layerSet = GetSetByName("Output Folder", doc, selBorad);
        outputPath = outputPath + "/" + selBorad.name
        var fso = new Folder(outputPath);
        if (!fso.exists) 
        {
            fso.create();
        } 
        OutputLayers2PNG(layerSet, outputPath);
        dialog.close();
        alert("已将PNG文件输出至UNITY资源目录！")
    }
}

function GetSetByName(name, doc, board){
    var layerSet = board.layerSets.getByName(name);
    return layerSet;
}

function OutputLayers2PNG(layerSet, path){
    if (layerSet.layerSets.length > 0)
    {
        for (var i = 0; i < layerSet.layerSets.length; i++)
        {
            var child = layerSet.layerSets[i];
            var tempPath = path + "/" + child.name
            OutputLayers2PNG(child, tempPath);
        }
    }
    else
    {
        var fso = new Folder(path);
        if (!fso.exists) {fso.create();} 
        for (var i = 0; i < layerSet.layers.length; i++)
        {
            var layer = layerSet.layers[i];
            saveLayer(layer, layer.name, path + "/");
        }
    }
}

function saveLayer(layer, lname, path) {
    app.activeDocument.activeLayer = layer;
    dupLayers();
    app.activeDocument.trim(TrimType.TRANSPARENT,true,true,true,true);
    var saveFile= File(path + lname + ".png");
    SavePNG(saveFile);
    app.activeDocument.close(SaveOptions.DONOTSAVECHANGES);
}

function dupLayers() { 
    var desc143 = new ActionDescriptor();
        var ref73 = new ActionReference();
        ref73.putClass( charIDToTypeID('Dcmn') );
    desc143.putReference( charIDToTypeID('null'), ref73 );
    desc143.putString( charIDToTypeID('Nm  '), activeDocument.activeLayer.name );
        var ref74 = new ActionReference();
        ref74.putEnumerated( charIDToTypeID('Lyr '), charIDToTypeID('Ordn'), charIDToTypeID('Trgt') );
    desc143.putReference( charIDToTypeID('Usng'), ref74 );
    executeAction( charIDToTypeID('Mk  '), desc143, DialogModes.NO );
};

function SavePNG(saveFile){
    var pngOpts = new ExportOptionsSaveForWeb; 
    pngOpts.format = SaveDocumentType.PNG
    pngOpts.PNG8 = false; 
    pngOpts.transparency = true; 
    pngOpts.interlaced = false; 
    pngOpts.quality = 100;
    app.activeDocument.exportDocument(new File(saveFile),ExportType.SAVEFORWEB,pngOpts); 
}

function savePath(path){
    alert("资源路径更新: " + path)
    settings.psdDir = path;
    saveSettings();
}

function getOptionType (value) {
	switch (typeof(value)) {
	case "boolean": return "Boolean";
	case "string": return "String";
	case "number": return "Double";
	};
	throw new Error("Invalid default setting: " + value);
}

function trim (value) {
	return value.replace(/^\s+|\s+$/g, "");
}

function startsWith (str, prefix) {
	return str.indexOf(prefix) === 0;
}

function endsWith (str, suffix) {
	return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

function quote (value) {
	return '"' + value.replace(/"/g, '\\"') + '"';
}
