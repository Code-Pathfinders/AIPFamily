<template>
  <div class="family-tree-container">
    <div ref="containerRef" class="graph-container"></div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue'
import G6, { Graph, TreeGraph } from '@antv/g6'
import type { FamilyMember, G6NodeData, G6EdgeData, GraphDisplayConfig, CardFieldConfig } from '@/types'

/**
 * 组件属性
 */
interface Props {
  members: FamilyMember[]
  displayConfig?: GraphDisplayConfig
}

const props = withDefaults(defineProps<Props>(), {
  displayConfig: () => ({
    layoutDirection: 'TB',
    cardFields: {
      showName: true,
      showGeneration: true,
      showBirthYear: true,
      showGender: false,
      showAvatar: true
    },
    nodeWidth: 150,
    nodeHeight: 80,
    nodeSpacing: 50,
    rankSpacing: 100,
    edgeStyle: {
      fatherLineColor: '#5B8FF9',
      motherLineColor: '#F759AB',
      lineWidth: 2.5,
      fatherLineStyle: 'solid',
      motherLineStyle: 'dashed',
      showShadow: true
    }
  })
})

/**
 * 事件定义
 */
const emit = defineEmits<{
  nodeClick: [member: FamilyMember]
  nodeDoubleClick: [member: FamilyMember]
  positionChange: [positions: Array<{ id: string; x: number; y: number }>]
}>()

const containerRef = ref<HTMLDivElement>()
let graph: Graph | null = null

/**
 * 初始化图形
 */
const initGraph = () => {
  if (!containerRef.value) return

  const width = containerRef.value.offsetWidth
  const height = containerRef.value.offsetHeight

  // 注册自定义节点
  G6.registerNode('family-member', {
    draw(cfg: any, group: any) {
      const { member, config } = cfg
      const fieldConfig = config?.cardFields || props.displayConfig.cardFields
      const nodeWidth = config?.nodeWidth || props.displayConfig.nodeWidth
      const nodeHeight = config?.nodeHeight || props.displayConfig.nodeHeight
      
      const nodeStyle = member.nodeStyle ? JSON.parse(member.nodeStyle) : {}
      
      // 默认样式
      const defaultStyle = {
        fill: member.gender === 'Male' ? '#5B8FF9' : '#F759AB',
        stroke: member.gender === 'Male' ? '#5B8FF9' : '#F759AB',
        lineWidth: 2,
        opacity: 0.9
      }

      const style = { ...defaultStyle, ...nodeStyle }

      // 绘制主形状
      const shape = group.addShape('rect', {
        attrs: {
          x: -nodeWidth / 2,
          y: -nodeHeight / 2,
          width: nodeWidth,
          height: nodeHeight,
          radius: 8,
          ...style
        },
        name: 'main-box',
        draggable: true
      })

      let currentY = -nodeHeight / 2 + 15
      const leftX = -nodeWidth / 2 + 10
      const centerX = 0

      // 添加头像（如果配置显示）
      let hasAvatar = false
      if (fieldConfig.showAvatar && member.avatarUrl) {
        hasAvatar = true
        group.addShape('image', {
          attrs: {
            x: leftX,
            y: currentY,
            width: 40,
            height: 40,
            img: member.avatarUrl,
            radius: 20
          },
          name: 'avatar'
        })
      }

      const textX = hasAvatar ? leftX + 50 : centerX
      // 如果没有头像，从中间开始，留出更多空间
      if (!hasAvatar) {
        currentY = -nodeHeight / 2 + 25
      }

      // 添加姓名（如果配置显示）
      if (fieldConfig.showName) {
        group.addShape('text', {
          attrs: {
            x: textX,
            y: hasAvatar ? currentY + 10 : currentY,
            text: member.name,
            fontSize: 16,
            fontWeight: 'bold',
            fill: '#fff',
            textAlign: hasAvatar ? 'left' : 'center',
            textBaseline: 'middle'
          },
          name: 'name-text'
        })
        currentY += 22
      }

      // 添加性别（如果配置显示）
      if (fieldConfig.showGender) {
        const genderText = member.gender === 'Male' ? '男' : '女'
        group.addShape('text', {
          attrs: {
            x: textX,
            y: currentY,
            text: genderText,
            fontSize: 12,
            fill: '#fff',
            textAlign: hasAvatar ? 'left' : 'center',
            textBaseline: 'middle'
          },
          name: 'gender-text'
        })
        currentY += 16
      }

      // 添加世代信息（如果配置显示）
      if (fieldConfig.showGeneration) {
        group.addShape('text', {
          attrs: {
            x: textX,
            y: currentY,
            text: `第${member.generation}代`,
            fontSize: 12,
            fill: '#fff',
            textAlign: hasAvatar ? 'left' : 'center',
            textBaseline: 'middle'
          },
          name: 'generation-text'
        })
        currentY += 16
      }

      // 添加出生年份（如果配置显示且有数据）
      if (fieldConfig.showBirthYear && member.birthDate) {
        const year = new Date(member.birthDate).getFullYear()
        group.addShape('text', {
          attrs: {
            x: textX,
            y: currentY,
            text: `${year}年`,
            fontSize: 11,
            fill: '#fff',
            opacity: 0.9,
            textAlign: hasAvatar ? 'left' : 'center',
            textBaseline: 'middle'
          },
          name: 'birth-year'
        })
      }

      // 添加自定义字段1
      if (fieldConfig.showCustomField1 && member.customAttributes) {
        try {
          const customAttrs = JSON.parse(member.customAttributes)
          const value = customAttrs[fieldConfig.showCustomField1]
          if (value) {
            currentY += 16
            group.addShape('text', {
              attrs: {
                x: textX,
                y: currentY,
                text: `${value}`,
                fontSize: 10,
                fill: '#fff',
                opacity: 0.8,
                textAlign: hasAvatar ? 'left' : 'center',
                textBaseline: 'middle'
              },
              name: 'custom-field-1'
            })
          }
        } catch (e) {
          // 忽略JSON解析错误
        }
      }

      return shape
    },
    update: undefined
  }, 'single-node')

  // 创建图实例
  graph = new G6.Graph({
    container: containerRef.value,
    width,
    height,
    modes: {
      default: ['drag-canvas', 'zoom-canvas', 'drag-node']
    },
    defaultNode: {
      type: 'family-member',
      size: [props.displayConfig.nodeWidth, props.displayConfig.nodeHeight]
    },
    defaultEdge: {
      type: props.displayConfig.layoutDirection === 'TB' || props.displayConfig.layoutDirection === 'BT' 
        ? 'cubic-vertical' 
        : 'cubic-horizontal',
      style: {
        stroke: '#5B8FF9',
        lineWidth: 2.5,
        opacity: 0.8,
        shadowColor: 'rgba(91, 143, 249, 0.2)',
        shadowBlur: 10,
        endArrow: {
          path: G6.Arrow.triangle(10, 12, 0),
          fill: '#5B8FF9',
          opacity: 0.8
        }
      }
    },
    layout: {
      type: 'dagre',
      rankdir: props.displayConfig.layoutDirection,
      align: 'UL',
      nodesep: props.displayConfig.nodeSpacing,
      ranksep: props.displayConfig.rankSpacing,
      // 允许处理没有边的节点
      controlPoints: true
    },
    // 启用节点的固定位置
    fitView: true,
    fitViewPadding: 20
  })

  // 监听节点点击事件
  graph.on('node:click', (e: any) => {
    const member = e.item.getModel().member
    emit('nodeClick', member)
  })

  // 监听节点双击事件
  graph.on('node:dblclick', (e: any) => {
    const member = e.item.getModel().member
    emit('nodeDoubleClick', member)
  })

  // 监听节点拖拽结束
  graph.on('node:dragend', () => {
    savePositions()
  })

  // 自适应画布
  window.addEventListener('resize', handleResize)
}

/**
 * 处理窗口大小变化
 */
const handleResize = () => {
  if (!graph || !containerRef.value) return
  
  const width = containerRef.value.offsetWidth
  const height = containerRef.value.offsetHeight
  graph.changeSize(width, height)
  graph.fitView(20)
}

/**
 * 构建图数据
 */
const buildGraphData = () => {
  const nodes: G6NodeData[] = []
  const edges: G6EdgeData[] = []

  props.members.forEach((member, index) => {
    // 添加节点
    const node: G6NodeData = {
      id: member.id,
      label: member.name,
      member: member,
      config: props.displayConfig
    } as any

    // 如果有保存的位置，使用保存的位置
    if (member.positionX !== undefined && member.positionY !== undefined) {
      node.x = member.positionX
      node.y = member.positionY
    } else {
      // 如果没有保存的位置，为独立节点设置默认位置
      // 按照世代和索引排列
      const generation = member.generation || 1
      const xOffset = index * 200
      node.x = 100 + xOffset
      node.y = 100 + (generation - 1) * 150
    }

    nodes.push(node)

    // 添加与父亲的边
    if (member.fatherId) {
      const edgeStyle = props.displayConfig.edgeStyle
      const fatherColor = edgeStyle.fatherLineColor
      const shadowColor = `rgba(${parseInt(fatherColor.slice(1, 3), 16)}, ${parseInt(fatherColor.slice(3, 5), 16)}, ${parseInt(fatherColor.slice(5, 7), 16)}, 0.2)`
      
      edges.push({
        id: `${member.fatherId}-${member.id}`,
        source: member.fatherId,
        target: member.id,
        type: props.displayConfig.layoutDirection === 'TB' || props.displayConfig.layoutDirection === 'BT' 
          ? 'cubic-vertical' 
          : 'cubic-horizontal',
        style: {
          stroke: fatherColor,
          lineWidth: edgeStyle.lineWidth,
          opacity: 0.8,
          ...(edgeStyle.fatherLineStyle === 'dashed' ? { lineDash: [8, 4] } : {}),
          ...(edgeStyle.showShadow ? { shadowColor, shadowBlur: 10 } : {}),
          endArrow: {
            path: G6.Arrow.triangle(10, 12, 0),
            fill: fatherColor,
            opacity: 0.8
          }
        }
      })
    }

    // 添加与母亲的边
    if (member.motherId) {
      const edgeStyle = props.displayConfig.edgeStyle
      const motherColor = edgeStyle.motherLineColor
      const shadowColor = `rgba(${parseInt(motherColor.slice(1, 3), 16)}, ${parseInt(motherColor.slice(3, 5), 16)}, ${parseInt(motherColor.slice(5, 7), 16)}, 0.2)`
      
      edges.push({
        id: `${member.motherId}-${member.id}`,
        source: member.motherId,
        target: member.id,
        type: props.displayConfig.layoutDirection === 'TB' || props.displayConfig.layoutDirection === 'BT' 
          ? 'cubic-vertical' 
          : 'cubic-horizontal',
        style: {
          stroke: motherColor,
          lineWidth: edgeStyle.lineWidth,
          opacity: 0.8,
          ...(edgeStyle.motherLineStyle === 'dashed' ? { lineDash: [8, 4] } : {}),
          ...(edgeStyle.showShadow ? { shadowColor, shadowBlur: 10 } : {}),
          endArrow: {
            path: G6.Arrow.triangle(10, 12, 0),
            fill: motherColor,
            opacity: 0.8
          }
        }
      })
    }
  })

  return { nodes, edges }
}

/**
 * 渲染图形
 */
const renderGraph = () => {
  if (!graph) return

  const data = buildGraphData()
  graph.data(data)
  graph.render()
  
  // 自适应视图
  nextTick(() => {
    graph?.fitView(20)
  })
}

/**
 * 保存节点位置
 */
const savePositions = () => {
  if (!graph) return

  const positions: Array<{ id: string; x: number; y: number }> = []
  const nodes = graph.getNodes()

  nodes.forEach(node => {
    const model = node.getModel()
    positions.push({
      id: model.id as string,
      x: model.x as number,
      y: model.y as number
    })
  })

  emit('positionChange', positions)
}

/**
 * 销毁图形
 */
const destroyGraph = () => {
  if (graph) {
    window.removeEventListener('resize', handleResize)
    graph.destroy()
    graph = null
  }
}

// 监听成员数据变化
watch(() => props.members, () => {
  renderGraph()
}, { deep: true })

// 监听配置变化
watch(() => props.displayConfig, () => {
  // 配置变化时需要重新初始化图形
  destroyGraph()
  initGraph()
  renderGraph()
}, { deep: true })

// 组件挂载
onMounted(() => {
  initGraph()
  renderGraph()
})

// 组件卸载
onUnmounted(() => {
  destroyGraph()
})
</script>

<style scoped>
.family-tree-container {
  width: 100%;
  height: 100%;
  position: relative;
}

.graph-container {
  width: 100%;
  height: 100%;
  background: linear-gradient(to bottom, #f0f2f5, #ffffff);
}
</style>
