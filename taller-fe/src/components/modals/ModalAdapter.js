import { __awaiter, __generator } from "tslib";
import Swal from 'sweetalert2';
// Adaptador para Swal
function showAlert(_a) {
    return __awaiter(this, arguments, void 0, function (_b) {
        var result;
        var title = _b.title, _c = _b.text, text = _c === void 0 ? '' : _c, _d = _b.icon, icon = _d === void 0 ? 'info' : _d, // 'info', 'success', 'error', 'warning', 'question'
        _e = _b.confirmButtonText, // 'info', 'success', 'error', 'warning', 'question'
        confirmButtonText = _e === void 0 ? 'Aceptar' : _e, _f = _b.cancelButtonText, cancelButtonText = _f === void 0 ? 'Cancelar' : _f, _g = _b.showCancelButton, showCancelButton = _g === void 0 ? false : _g;
        return __generator(this, function (_h) {
            switch (_h.label) {
                case 0: return [4 /*yield*/, Swal.fire({
                        title: title,
                        text: text,
                        icon: icon,
                        showCancelButton: showCancelButton,
                        confirmButtonText: confirmButtonText,
                        cancelButtonText: cancelButtonText,
                    })];
                case 1:
                    result = _h.sent();
                    return [2 /*return*/, result.isConfirmed]; // Devuelve `true` si el usuario confirma, o `false` si cancela
            }
        });
    });
}
// Modal de éxito
function OkModal(title, text) {
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, showAlert({
                        title: title,
                        text: text,
                        icon: 'success',
                    })];
                case 1:
                    _a.sent();
                    return [2 /*return*/];
            }
        });
    });
}
// Modal de error
function errorModal(title, text) {
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, showAlert({
                        title: title,
                        text: text,
                        icon: 'error',
                    })];
                case 1:
                    _a.sent();
                    return [2 /*return*/];
            }
        });
    });
}
function deleteModal(title, text) {
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, showAlert({
                        title: title,
                        text: text,
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonText: '<i class="fas fa-trash"></i> Sí, borrar',
                        cancelButtonText: '<i class="fas fa-times"></i> Cancelar'
                    })];
                case 1:
                    _a.sent();
                    return [2 /*return*/];
            }
        });
    });
}
function personalizadoModal(html, title, text) {
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, showAlert({
                        title: title,
                        text: text,
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonText: '<i class="fas fa-trash"></i> Sí, borrar',
                        cancelButtonText: '<i class="fas fa-times"></i> Cancelar'
                    })];
                case 1:
                    _a.sent();
                    return [2 /*return*/];
            }
        });
    });
}
// Exportar funciones
export { showAlert, OkModal, errorModal, deleteModal };
//# sourceMappingURL=ModalAdapter.js.map