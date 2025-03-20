import Swal from 'sweetalert2';

// Adaptador para Swal
async function showAlert({
  title,
  text = '',
  icon = 'info', // 'info', 'success', 'error', 'warning', 'question'
  confirmButtonText = 'Aceptar',
  cancelButtonText = 'Cancelar',
  showCancelButton = false, // Determina si muestra el botón de cancelar
}: {
  title: string;
  text?: string;
  icon?: 'info' | 'success' | 'error' | 'warning' | 'question';
  confirmButtonText?: string;
  cancelButtonText?: string;
  showCancelButton?: boolean;
}): Promise<boolean> {
  const result = await Swal.fire({
    title,
    text,
    icon,
    showCancelButton,
    confirmButtonText,
    cancelButtonText,
  });

  return result.isConfirmed; // Devuelve `true` si el usuario confirma, o `false` si cancela
}

// Modal de éxito
async function OkModal(title: string, text?: string) {
  await showAlert({
    title,
    text,
    icon: 'success',
  });
}

// Modal de error
async function errorModal(title: string, text?: string) {
  await showAlert({
    title,
    text,
    icon: 'error',
  });
}

// Exportar funciones
export { showAlert, OkModal, errorModal };