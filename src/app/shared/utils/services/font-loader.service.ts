import { Injectable, RendererFactory2, inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FontLoaderService {
  renderer: any;
  constructor() {
    const rendererFactory2 = inject(RendererFactory2);
    this.renderer = rendererFactory2.createRenderer(null, null)
  }
  public loadFont(font: string) {
    const formatted = font.replace(/ /g, '+');
    const href = `https://fonts.googleapis.com/css2?family=${formatted}:wght@100;200;300;400;500;600;700;800;900&&display=swap`;
    // Agrega fuente dinámica si no existe aún
    const dynamicFontExists = Array.from(document.head.querySelectorAll('link')).some(link =>
      link.href.includes(href)
    );
    if (!dynamicFontExists) {
      const link = this.renderer.createElement('link');
      this.renderer.setAttribute(link, 'rel', 'stylesheet');
      this.renderer.setAttribute(link, 'href', href);
      this.renderer.appendChild(document.head, link);
    }
  }



}
