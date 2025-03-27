const Form = load.class('form');

class MainForm extends Form {

    getSchema() {
        return {
            name: 'main',
            title: 'My Electron App',
            left: 400,
            top: 100,
            width: 640,
            height: 480,
            resizable: true,
            maximizable: true,
            minimizable: true
        };
    }

    buildComponents() {
        this.buildComponentsFromSchemaList([])
    }

}

module.exports = MainForm;