export const zhCN = {
  app: {
    title: 'Hstar Canvas',
    subtitle: '第四代 AIGC 无限画布工作台'
  },
  commands: {
    runWorkflow: '运行工作流',
    save: '保存',
    open: '打开',
    providerSettings: '服务商设置'
  },
  panels: {
    nodeLibrary: '节点库',
    inspector: '属性',
    taskQueue: '任务队列',
    outputGallery: '输出画廊'
  },
  nodes: {
    prompt: '提示词',
    imageGenerate: '图像生成',
    output: '输出画廊',
    assetImage: '素材图片'
  },
  status: {
    ready: '就绪',
    emptyInspector: '选择节点后编辑参数。',
    emptyOutput: '生成结果会显示在这里。'
  }
} as const;

