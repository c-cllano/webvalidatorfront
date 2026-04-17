import Swal from "sweetalert2";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class AlertMessageService {

  close() {
    Swal.close();
  }

  public Loading() {
    Swal.fire({
      text: 'Procesando...',
      allowOutsideClick: false,
    });
    Swal.showLoading();
  }

  public Success(message: string, icon: any = "success") {
    const Toast = Swal.mixin({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 1500,
      timerProgressBar: true,
      didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
      }
    })
    Toast.fire({
      icon: icon,
      title: message
    })
  }


  public SuccessNotTime(message: string) {
    Swal.fire({
      text: message,
      icon: 'success',
      title: '¡Éxito!',
    })
  }

  public Message(text: string, label: string, value: string) {
    Swal.fire({
      text: text,
      icon: 'success',
      title: '¡Éxito!',
      input: 'text',
      inputLabel: label,
      inputValue: value,
    })
  }




  public Error(message: string) {
    Swal.fire({
      icon: 'error',
      title: '¡Error!',
      text: message,
    });

  }

  public Info(message: string) {
    Swal.fire({
      icon: 'info',
      title: '¡Información!',
      text: message,
    });

  }

  public Warning(message: string) {
    Swal.fire({
      icon: 'warning',
      title: '¡Atención!',
      text: message,
    });

  }

  public Confirm(message: string) {
    return new Promise(function (resolve) {
      Swal.fire({
        text: message,
        title: '¡Atención!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si',
        cancelButtonText: 'No',
        allowOutsideClick: false,
        allowEscapeKey: false,
        allowEnterKey: false,
        showCloseButton: false
      }).then((result) => {
        resolve(result);
        Swal.close();
      });
    });
  }


  public MessageButton(title: string, Text1: string = "", Text1Color: string = "", Text2: string = "", Text2Color: string = "", Text3: string = "", Text3Color: string = "") {
    return new Promise(function (resolve) {
      Swal.fire({
        title: title,
        showDenyButton: (Text2 != "" ? true : false),
        showCancelButton: (Text3 != "" ? true : false),
        confirmButtonText: Text1,
        denyButtonText: Text2,
        cancelButtonText: Text3,
        confirmButtonColor: Text1Color,
        denyButtonColor: Text2Color,
        cancelButtonColor: Text3Color
      }).then((result) => {
        if (result.isConfirmed) {
          resolve('Text1');
        } else if (result.isDenied) {
          resolve('Text2');
        } else if (result.dismiss === Swal.DismissReason.cancel) {
          resolve('Text3');
        }
      });
    });
  }

  public MessageInput(text: any, title: any, icon: any, input: any, inputPlaceholder: any, maxlength: any) {
    return new Promise(function (resolve) {
      Swal.fire({
        title: title,
        text: text,
        icon: icon,
        input: input,
        inputPlaceholder: inputPlaceholder,
        inputAttributes: { 'maxlength': maxlength },
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si',
        cancelButtonText: 'No',
        customClass: {
          validationMessage: 'my-validation-message'
        },
        preConfirm: (value) => {
          if (!value) {
            Swal.showValidationMessage(
              '<i class="fa fa-info-circle"></i> Este campo es requerido.'
            )
          } else {
            const format = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;
            if (format.test(value)) {
              Swal.showValidationMessage('<i class="fa fa-info-circle"></i> Caracteres especiales no están permitidos.');
            } else {
              resolve(value);
              Swal.close();
            }
          }
        }
      }).then((result) => {
        if (result.dismiss === Swal.DismissReason.cancel) {
          resolve("");
        }
      });
    });
  }


  public MessageRadio(Title: string, inputOptions: any) {
    return new Promise(async function (resolve) {
      Swal.fire({
        title: Title,
        input: "radio",
        inputOptions: inputOptions,
        showCancelButton: true,
        inputValidator: (value) => {
          return !value && "Este campo es requerido.";
        }
      }).then((result) => {
        resolve(result);
      });
    });
  }


}
