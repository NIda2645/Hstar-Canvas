import { FolderOpen, Play, Save, Settings } from 'lucide-react';
import './App.css';
import { zhCN } from './i18n/zhCN';

export default function App() {
  return (
    <main className='app-shell'>
      <aside className='left-panel'>
        <h2>{zhCN.panels.nodeLibrary}</h2>
        <button>{zhCN.nodes.prompt}</button>
        <button>{zhCN.nodes.imageGenerate}</button>
        <button>{zhCN.nodes.output}</button>
        <button>{zhCN.nodes.assetImage}</button>
      </aside>

      <section className='workspace'>
        <div className='command-bar'>
          <div>
            <strong>{zhCN.app.title}</strong>
            <span>{zhCN.app.subtitle}</span>
          </div>
          <nav aria-label='工作流命令'>
            <button title={zhCN.commands.open}><FolderOpen size={16} />{zhCN.commands.open}</button>
            <button title={zhCN.commands.save}><Save size={16} />{zhCN.commands.save}</button>
            <button title={zhCN.commands.providerSettings}><Settings size={16} />{zhCN.commands.providerSettings}</button>
            <button className='primary' title={zhCN.commands.runWorkflow}><Play size={16} />{zhCN.commands.runWorkflow}</button>
          </nav>
        </div>
        <div className='canvas-placeholder'><span>{zhCN.status.ready}</span></div>
        <footer className='bottom-panel'>
          <section><h2>{zhCN.panels.taskQueue}</h2><p>{zhCN.status.ready}</p></section>
          <section><h2>{zhCN.panels.outputGallery}</h2><p>{zhCN.status.emptyOutput}</p></section>
        </footer>
      </section>

      <aside className='right-panel'>
        <h2>{zhCN.panels.inspector}</h2>
        <p>{zhCN.status.emptyInspector}</p>
      </aside>
    </main>
  );
}
