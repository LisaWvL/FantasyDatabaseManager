const Controller = load.class('controller');

class MainController extends Controller {

    start() {
        this.createFormWindow('main');
    }

}

module.exports = MainController;